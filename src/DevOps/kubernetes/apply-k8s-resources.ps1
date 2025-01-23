kubectl apply -f 06-framepack-api-deployment.yaml
kubectl apply -f 07-framepack-api-service.yaml
kubectl apply -f 08-framepack-api-hpa.yaml

# Esperar o pod antes de iniciar o port-forward
while ($true) {
    $podStatus = kubectl get pods -l app=framepack-api -o jsonpath='{.items[*].status.phase}'
    $podStatusArray = $podStatus -split ' '
    if ($podStatusArray -contains 'Running') {
        Write-Output "Pod framepack-api esta em execucao."
        break
    }
    else {
        Write-Output "Status atual do pod framepack-api: $podStatus"
    }
    Write-Output "Esperando o pod framepack-api estar em execucao..."
    Start-Sleep -Seconds 5
}

# Obter o nome do pod
$podName = kubectl get pods -l app=framepack-api -o jsonpath='{.items[0].metadata.name}'

# Verificar se o pod está realmente rodando
if ($podStatusArray -contains 'Running') {
    kubectl port-forward svc/framepack-api 8080:80
}
else {
    Write-Output "O pod nao esta em execucao. Verificando logs"
    kubectl describe pod $podName
    kubectl logs $podName
}
