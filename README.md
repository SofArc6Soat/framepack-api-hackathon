- [Aplicação Framepack-WebApi (backend)](#aplicação-framepack-webapi)
  - [Funcionalidades Principais](#funcionalidades-principais)
  - [Estrutura do Projeto](#estrutura-do-projeto)
  - [Tecnologias Utilizadas](#tecnologias-utilizadas)
  - [Serviços Utilizados](#serviços-utilizados)
- [Aplicação Framepack-Worker](#aplicação-framepack-worker)
  - [Funcionalidades Principais](#funcionalidades-principais-1)
  - [Estrutura do Projeto](#estrutura-do-projeto-1)
  - [Tecnologias Utilizadas](#tecnologias-utilizadas-1)
  - [Serviços Utilizados](#serviços-utilizados-1)
  - [Como Executar o Projeto](#como-executar-o-projeto)
    - [Clonar o repositório](#clonar-o-repositório)
    - [Executar com docker-compose](#executar-com-docker-compose)
    - [Executar com Kubernetes](#executar-com-kubernetes)
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

- **Api**: Contém a API principal do projeto.
- **BuildingBlocks**: Contém serviços e utilitários comuns, como o serviço de integração com o S3.
- **Controllers**: Contém os controladores responsáveis por lidar com as requisições HTTP.
- **DevOps**: Contém scripts e configurações para Docker e Kubernetes.
- **Domain**: Contém as entidades e regras de negócio do domínio.
- **Gateways**: Contém os handlers responsáveis pelo processamento de vídeos.
- **Gateways.Cognito**: Contém a integração com o Amazon Cognito para autenticação e autorização.
- **Infra**: Contém a infraestrutura necessária para o funcionamento do projeto, como configurações de banco de dados e serviços externos.
- **UseCases**: Contém os casos de uso principais do worker.

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

---

# Aplicação Framepack-Worker

O projeto Framepack-Worker é responsável pelo processamento assíncrono de vídeos, extraindo frames e realizando outras operações de backend necessárias para o funcionamento do sistema.

## Funcionalidades Principais

- **Processamento de Vídeos**: Processa vídeos, extrai frames e retorna as imagens em um arquivo .zip.
- **Fila de Processamento**: Utilização do Amazon SQS para gerenciar a fila de processamento dos vídeos.
- **Armazenamento de Arquivos**: Utilização do Amazon S3 para armazenar os vídeos enviados e os arquivos .zip gerados.
- **Extração de Frames de Vídeos**: Extrai frames dos vídeos baixados em intervalos específicos.

## Estrutura do Projeto

- **BuildingBlocks**: Contém serviços e utilitários comuns, como o serviço de integração com o S3.
- **Controllers**: Contém os controladores responsáveis por lidar com as requisições HTTP.
- **DevOps**: Contém scripts e configurações para Docker e Kubernetes.
- **Gateways**: Contém os handlers responsáveis pelo processamento de vídeos.
- **Infra**: Contém a infraestrutura necessária para o funcionamento do projeto, como configurações de banco de dados e serviços externos.
- **UseCases**: Contém os casos de uso principais do worker.
- **Worker**: Contém a lógica principal do worker para download de vídeos, extração de frames, compactação e upload.

## Tecnologias Utilizadas

- **.NET 8**: Framework principal para desenvolvimento do backend. <br>
- **Docker**: Containerização da aplicação para garantir portabilidade e facilitar o deploy. <br>
- **Kubernetes**: Orquestração dos container visando resiliência da aplicação <br>
- **Banco de Dados**: Utilização do SQL Server para armazenamento de informações. <br>

## Serviços Utilizados

- **Amazon SQS**: Para gerenciar a fila de processamento dos vídeos.
- **Amazon S3**: Para armazenar os vídeos enviados e os arquivos .zip gerados.
- **Github Actions**: Todo o CI/CD é automatizado através de pipelines. <br>
- **SonarQube**: Análise estática do código para promover qualidade. <br>

---

## Como Executar o Projeto (Framepack-WebApi)

### Clonar o repositório
  ```
  git clone https://github.com/SofArc6Soat/framepack-api-hackathon.git
  ```

### Executar com docker-compose
#### Docker (docker-compose)
- **Navegue até o diretório do projeto:**
  ```
  cd framepack-api-hackathon\src\DevOps
  ```
- **Configure o ambiente Docker:**
  ```
  docker-compose up --build
  ```
- **A aplicação estará disponível em:** http://localhost:5001
- **URL do Swagger:** http://localhost:5001/swagger
- **URL do Healthcheck da API:** http://localhost:5001/health

### Executar com Kubernetes
#### Kubernetes

Para executar o projeto com Kubernetes, siga os passos abaixo:

- **Crie um arquivo `.env`** no diretório raiz do projeto (`framepack-api-hackathon/src/DevOps/kubernetes/`) e configure as variáveis de ambiente necessárias:

  ```plaintext
  AWS_ACCESS_KEY_ID=your_access_key_id
  AWS_SECRET_ACCESS_KEY=your_secret_access_key
  AWS_REGION=your_region
  ```

- **Navegue até o diretório do projeto**:
  ```
  cd framepack-api-hackathon\src\DevOps\kubernetes
  ```
  
- **Crie um Secret no Kubernetes** a partir do arquivo [.env]:
  ```sh
  kubectl create secret generic aws-secret --from-env-file=framepack-api-hackathon/.env
  ```

- **Aplique os arquivos YAML** para configurar os recursos do Kubernetes:
  ```sh
  kubectl apply -f 01-dynamodb-local-deployment.yaml
  kubectl apply -f 02-dynamodb-local-service.yaml
  kubectl apply -f 03-dynamodb-local-setup-deployment.yaml
  kubectl apply -f 04-framepack-api-deployment.yaml
  kubectl apply -f 05-framepack-api-service.yaml
  kubectl apply -f 06-framepack-api-hpa.yaml
  ```

- **Aguarde até que os pods da API e do Worker estejam em execução**:
  ```sh
  kubectl get pods -l app=framepack-api
  ```

- **Configure o port-forwarding** para os serviços da API e do Worker:
  ```sh
  kubectl port-forward svc/framepack-api-service 8080:80 8443:443
  ```

### Usando o Script PowerShell

Se preferir, você pode executar o script PowerShell que automatiza todos os passos acima:

- **Crie um arquivo [.env]** no diretório raiz do projeto (`framepack-api-hackathon/src/DevOps/kubernetes/`) e configure as variáveis de ambiente necessárias:

  ```plaintext
  AWS_ACCESS_KEY_ID=your_access_key_id
  AWS_SECRET_ACCESS_KEY=your_secret_access_key
  AWS_REGION=your_region
  ```
- **Execute o script PowerShell** para criar o Secret e aplicar os recursos do Kubernetes:

  ```powershell
  Get-ExecutionPolicy
  Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
  .\delete-k8s-resources.ps1
  .\apply-k8s-resources.ps1
  ```
Este script irá:

- Criar um Secret no Kubernetes a partir do arquivo [.env].
- Aplicar todos os arquivos YAML necessários para configurar os recursos do Kubernetes.
- Aguardar até que os pods da API e do Worker estejam em execução.
- Configurar o port-forwarding para os serviços da API e do Worker.

---

## Como Executar o Projeto (Framepack-Worker)

### Clonar o repositório
  ```
  git clone https://github.com/SofArc6Soat/framepack-worker-hackathon.git
  ```

### Executar com docker-compose
#### Docker (docker-compose)
- **Navegue até o diretório do projeto:**
  ```
  cd framepack-worker-hackathon\src\DevOps
  ```
- **Configure o ambiente Docker:**
  ```
  docker-compose up --build
  ```
- **A aplicação estará disponível em:** http://localhost:5001
- **URL do Swagger:** http://localhost:5001/swagger
- **URL do Healthcheck da API:** http://localhost:5001/health

### Executar com Kubernetes
#### Kubernetes

Para executar o projeto com Kubernetes, siga os passos abaixo:

- **Crie um arquivo `.env`** no diretório (`framepack-worker-hackathon/src/DevOps/kubernetes/`) e configure as variáveis de ambiente necessárias:

  ```plaintext
  AWS_ACCESS_KEY_ID=your_access_key_id
  AWS_SECRET_ACCESS_KEY=your_secret_access_key
  AWS_REGION=your_region
  ```

- **Navegue até o diretório do projeto**:
  ```
  cd framepack-worker-hackathon\src\DevOps\kubernetes
  ```
  
- **Crie um Secret no Kubernetes** a partir do arquivo [.env]:
  ```sh
  kubectl create secret generic aws-secret --from-env-file=framepack-worker-hackathon/.env
  ```

- **Aplique os arquivos YAML** para configurar os recursos do Kubernetes:
  ```sh
  kubectl apply -f 01-framepack-worker-deployment.yaml
  kubectl apply -f 02-framepack-worker-service.yaml
  kubectl apply -f 03-framepack-worker-hpa.yaml
  ```

- **Aguarde até que os pods da API e do Worker estejam em execução**:
  ```sh
  kubectl get pods -l app=framepack-worker
  ```

- **Configure o port-forwarding** para os serviços da API e do Worker:
  ```sh
  kubectl port-forward svc/framepack-worker-service 8080:80
  ```

### Usando o Script PowerShell

Se preferir, você pode executar o script PowerShell que automatiza todos os passos acima:

- **Crie um arquivo [.env]** no diretório raiz do projeto (`framepack-worker-hackathon/src/DevOps/kubernetes/`) e configure as variáveis de ambiente necessárias:

  ```plaintext
  AWS_ACCESS_KEY_ID=your_access_key_id
  AWS_SECRET_ACCESS_KEY=your_secret_access_key
  AWS_REGION=your_region
  ```
- **Execute o script PowerShell** para criar o Secret e aplicar os recursos do Kubernetes:

  ```powershell
  Get-ExecutionPolicy
  Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
  .\delete-k8s-resources.ps1
  .\apply-k8s-resources.ps1
  ```
Este script irá:

- Criar um Secret no Kubernetes a partir do arquivo [.env].
- Aplicar todos os arquivos YAML necessários para configurar os recursos do Kubernetes.
- Aguardar até que os pods da API e do Worker estejam em execução.
- Configurar o port-forwarding para os serviços da API e do Worker.


**Certifique-se de ter o `kubectl` instalado e configurado corretamente em sua máquina antes de executar o script.**

---

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