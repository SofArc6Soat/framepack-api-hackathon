# Criar o Secret a partir do arquivo .env
kubectl create secret generic aws-secret --from-env-file=./.env

# Aplicar os arquivos YAML do DynamoDB
kubectl apply -f 01-dynamodb-local-deployment.yaml
kubectl apply -f 02-dynamodb-local-service.yaml
kubectl apply -f 03-dynamodb-local-setup-deployment.yaml

# Função para esperar um pod estar em execução
function WaitForPod {
    param (
        [string]$label
    )
    while ($true) {
        $podStatus = kubectl get pods -l app=$label -o jsonpath='{.items[*].status.phase}'
        $podStatusArray = $podStatus -split ' '
        if ($podStatusArray -contains 'Running') {
            Write-Output "Pod ${label} está em execução."
            break
        }
        else {
            Write-Output "Status atual do pod ${label}: ${podStatus}"
        }
        Write-Output "Esperando o pod ${label} estar em execução..."
        Start-Sleep -Seconds 5
    }
}

# Esperar o pod do DynamoDB estar em execução
WaitForPod -label "dynamodb-local"

# Aplicar os arquivos YAML do framepack-api
kubectl apply -f 04-framepack-api-deployment.yaml
kubectl apply -f 05-framepack-api-service.yaml
kubectl apply -f 06-framepack-api-hpa.yaml

# Esperar o pod da API estar em execução
WaitForPod -label "framepack-api"

# Obter o nome do pod da API
$podNameApi = kubectl get pods -l app=framepack-api -o jsonpath='{.items[0].metadata.name}'

# Verificar se o pod da API está realmente rodando
$podStatusApi = kubectl get pods -l app=framepack-api -o jsonpath='{.items[*].status.phase}' -split ' '
if ($podStatusApi -contains 'Running') {
    # Verificar o estado do contêiner dentro do pod
    $containerState = kubectl get pod $podNameApi -o jsonpath='{.status.containerStatuses[0].state}'
    if ($containerState -contains 'running') {
        kubectl port-forward svc/framepack-api-service 8080:80
    }
    elseif ($containerState -contains 'terminated') {
        Write-Output "O contêiner da API terminou. Reiniciando o pod."
        kubectl delete pod $podNameApi
        Start-Sleep -Seconds 5
        # Esperar o pod reiniciar
        WaitForPod -label "framepack-api"
    }
    else {
        Write-Output "O contêiner da API não está em execução. Verificando logs"
        kubectl describe pod $podNameApi
        kubectl logs $podNameApi
    }
}
else {
    Write-Output "O pod da API não está em execução. Verificando logs"
    kubectl describe pod $podNameApi
    kubectl logs $podNameApi
}