- [Aplicação Framepack-WebApi (backend)](#aplicação-framepack-webapi)
  - [Funcionalidades Principais](#funcionalidades-principais)
  - [Estrutura do Projeto](#estrutura-do-projeto)
  - [Tecnologias Utilizadas](#tecnologias-utilizadas)
  - [Serviços Utilizados](#serviços-utilizados)
  - [Motivações para Utilizar o SQL Server como Banco de Dados da Aplicação](#motivações-para-utilizar-o-sql-server-como-banco-de-dados-da-aplicação)
  - [Como Executar o Projeto](#como-executar-o-projeto)
    - [Clonar o repositório](#clonar-o-repositório)
    - [Executar com docker-compose](#executar-com-docker-compose)
    - [Executar com Kubernetes](#executar-com-kubernetes)
  - [Collection com todas as APIs com exemplos de requisição](#collection-com-todas-as-apis-com-exemplos-de-requisição)
  - [Desenho da arquitetura](#desenho-da-arquitetura)
  - [Demonstração em vídeo](#demonstração-em-vídeo)
  - [Relatório de Cobertura](#relatório-de-cobertura)
  - [Autores](#autores)
  - [Documentação Adicional](#documentação-adicional)
  - [Repositórios microserviços](#repositórios-microserviços)
  - [Repositórios diversos](#repositórios-diversos)

---

# Aplicação Framepack-WebApi (backend)

Este projeto visa o desenvolvimento do backend para um software de processamento de imagens. O software processa um vídeo, extrai os frames e retorna as imagens em um arquivo .zip.<br>
Utilizando a arquitetura limpa, .NET 8, SQL Server, Cognito, Amazon SQS, Docker e Kubernetes, o objetivo é criar uma base sólida e escalável para suportar as funcionalidades necessárias para um sistema de conversão de videos em frames. <br>
O foco principal é a criação de uma aplicação robusta, modular e de fácil manutenção.<br>
Este microserviço tem como pricipal objetivo ser responsável pelo cadastro de clientes, funcionários e produtos.<br>

## Funcionalidades Principais

- **Processamento de Vídeos**: Processa vídeos, extrai frames e retorna as imagens em um arquivo .zip.
- **Gerenciamento de Usuários**: Gestão de usuários (funcionários ou clientes) integrados com o Cognito, permitindo o cadastro, confirmação do e-mail e recuperação de senha.
- **Armazenamento de Dados**: Persistência de dados utilizando um banco de dados SQL Server.
- **Fila de Processamento**: Utilização do Amazon SQS para gerenciar a fila de processamento dos vídeos.
- **Armazenamento de Arquivos**: Utilização do Amazon S3 para armazenar os vídeos enviados e os arquivos .zip gerados.
- **Notificações por Email**: Envio de emails de notificação aos usuários sobre o status do processamento utilizando Amazon SES.
- **Containerização e Orquestração**: Utilização de Docker para containerização e Kubernetes para orquestração dos containers, garantindo portabilidade e resiliência da aplicação.
- **CI/CD Automatizado**: Automação de todo o CI/CD através de pipelines utilizando Github Actions.
- **Análise de Código**: Análise estática do código para promover qualidade utilizando SonarQube.

## Estrutura do Projeto

A arquitetura limpa será utilizada para garantir que a aplicação seja modular e de fácil manutenção, o projeto foi estruturado da seguinte forma: API, Controllers, Gateways, Gateways.Cognito, Presenters, Domain, Infra (implementação das interfaces de repositório, configurações de banco de dados) e Building Blocks (componentes e serviços reutilizáveis)<br>

## Tecnologias Utilizadas

- **.NET 8**: Framework principal para desenvolvimento do backend. <br>
- **Arquitetura Limpa**: Estruturação do projeto para promover a separação de preocupações e facilitar a manutenção e escalabilidade. <br>
- **Docker**: Containerização da aplicação para garantir portabilidade e facilitar o deploy. <br>
- **Kubernetes**: Orquestração dos container visando resiliência da aplicação <br>
- **Banco de Dados**: Utilização do SQL Server para armazenamento de informações. <br>

## Serviços Utilizados

- **Amazon SQS**: Para gerenciar a fila de processamento dos vídeos.
- **Amazon S3**: Para armazenar os vídeos enviados e os arquivos .zip gerados.
- **Amazon Cognito**: Para autenticação e autorização dos usuários.
- **Amazon SES**: Para envio de emails de notificação aos usuários sobre o status do processamento.
- **Github Actions**: Todo o CI/CD é automatizado através de pipelines. <br>
- **SonarQube**: Análise estática do código para promover qualidade. <br>

## Como Executar o Projeto

### Clonar o repositório
```
git clone https://github.com/SofArc6Soat/framepack-api-hackathon.git
```

### Executar com docker-compose
1. Docker (docker-compose)
1.1. Navegue até o diretório do projeto:
```
cd framepack-api-hackathon\src\DevOps
```
2. Configure o ambiente Docker:
```
docker-compose up --build
```
2.1. A aplicação estará disponível em http://localhost:5001
2.2. URL do Swagger: http://localhost:5001/swagger
2.3. URL do Healthcheck da API: http://localhost:5001/health

### Executar com Kubernetes
2. Kubernetes
2.1. Navegue até o diretório do projeto:
```
cd framepack-api-hackathon\src\DevOps\kubernetes
```
2.2. Configure o ambiente Docker:
```
kubectl apply -f 06-framepack-api-deployment.yaml
kubectl apply -f 07-framepack-api-service.yaml
kubectl apply -f 08-framepack-api-hpa.yaml
kubectl port-forward svc/framepack-api 8080:80
```
ou executar todos scripts via PowerShell
```
Get-ExecutionPolicy
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
.\delete-k8s-resources.ps1
.\apply-k8s-resources.ps1
```
2.3. A aplicação estará disponível em http://localhost:8080
2.4. URL do Swagger: http://localhost:8080/swagger
2.5. URL do Healthcheck da API: http://localhost:8080/health

## Collection com todas as APIs com exemplos de requisição
1. Caso deseje testar via postman com dados fake importe o arquivo...

2. Também é possível utilizar o Swagger disponibilizado pela aplicação (URL do Swagger: http://localhost:8080/swagger).

Para testar localmente com o Postman, a ordem de excução deverá ser a seguinte:

1. Caso deseje se autenticar pela API ao invés da função Lambda também é possível. Usuários para testes:

## Desenho da arquitetura
Para visualizar o desenho da arquitetura abra o arquivo "Arquitetura-Infra.drawio.png" e "Arquitetura-Macro.drawio.png" no diretório "arquitetura" ou importe o arquivo "Arquitetura.drawio" no Draw.io (https://app.diagrams.net/).

## Demonstração em vídeo
Para visualizar a demonstração da aplicação da Fase Hackathon:
- Atualizações efetuadas na arquitetura e funcionamento da aplicação - Link do Vimeo: 
- Processo de deploy e execução das pipelines - Link do Vimeo: 

## Autores

- **Anderson Lopez de Andrade RM: 350452** <br>
- **Henrique Alonso Vicente RM: 354583**<br>

## Documentação Adicional

- **Miro - Domain Storytelling, Context Map, Linguagem Ubíqua e Event Storming**: [Link para o Event Storming](https://miro.com/app/board/uXjVKST91sw=/)
- **Github - Domain Storytelling**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-domain-story-telling)
- **Github - Context Map**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
- **Github - Linguagem Ubíqua**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)

## Repositórios microserviços

- **Framepack-WebApi**: [Link](https://github.com/SofArc6Soat/framepack-api-hackathon)
- **Framepack-Worker**: [Link](https://github.com/SofArc6Soat/framepack-worker-hackathon)

## Repositórios diversos

- **Documentação**: [Link](https://github.com/SofArc6Soat/framepack-api)
- **Lambda Autenticação**: [Link](https://github.com/SofArc6Soat/quickfood-auth-function)