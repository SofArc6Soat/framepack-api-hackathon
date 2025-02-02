# Criar o Secret a partir do arquivo .env
kubectl create secret generic aws-secret --from-env-file=framepack-api-hackathon/.env

# Aplicar todos os arquivos YAML
kubectl apply -f 01-dynamodb-local-deployment.yaml
kubectl apply -f 02-dynamodb-local-service.yaml
kubectl apply -f 03-dynamodb-local-setup-deployment.yaml
kubectl apply -f 04-framepack-worker-deployment.yaml
kubectl apply -f 05-framepack-worker-hpa.yaml
kubectl apply -f 06-framepack-worker-service.yaml
kubectl apply -f 07-framepack-api-deployment.yaml
kubectl apply -f 08-framepack-api-service.yaml
kubectl apply -f 09-framepack-api-hpa.yaml

# Função para esperar um pod estar em execução
function WaitForPod {
    param (
        [string]$label
    )
    while ($true) {
        $podStatus = kubectl get pods -l app=$label -o jsonpath='{.items[*].status.phase}'
        $podStatusArray = $podStatus -split ' '
        if ($podStatusArray -contains 'Running') {
            Write-Output "Pod $label está em execução."
            break
        }
        else {
            Write-Output "Status atual do pod $label: $podStatus"
        }
        Write-Output "Esperando o pod $label estar em execução..."
        Start-Sleep -Seconds 5
    }
}

# Esperar os pods da API e do Worker estarem em execução
WaitForPod -label "framepack-api"
WaitForPod -label "framepack-worker"

# Obter o nome do pod da API
$podNameApi = kubectl get pods -l app=framepack-api -o jsonpath='{.items[0].metadata.name}'

# Verificar se o pod da API está realmente rodando
$podStatusApi = kubectl get pods -l app=framepack-api -o jsonpath='{.items[*].status.phase}' -split ' '
if ($podStatusApi -contains 'Running') {
    kubectl port-forward svc/framepack-api-service 8080:80
}
else {
    Write-Output "O pod da API não está em execução. Verificando logs"
    kubectl describe pod $podNameApi
    kubectl logs $podNameApi
}