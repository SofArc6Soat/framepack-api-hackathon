- [📌 Aplicação Framepack-WebApi (API)](#-aplicação-framepack-webapi-api)
  - [📖 Visão Geral](#-visão-geral)
    - [Fluxo de Processamento](#fluxo-de-processamento)
  - [🚀 Funcionalidades Principais](#-funcionalidades-principais)
      - [Upload de Vídeos](#upload-de-vídeos)
      - [Geração de Evento](#geração-de-evento)
      - [Download de Vídeos](#download-de-vídeos)
      - [Consultas de Status](#consultas-de-status)
      - [Gestão de Acessos](#gestão-de-acessos)
  - [📁 Estrutura do Projeto](#-estrutura-do-projeto)
  - [🛠 Tecnologias Utilizadas](#-tecnologias-utilizadas)
  - [▶️ Como Executar o Projeto (Framepack-WebApi)](#️-como-executar-o-projeto-framepack-webapi)
    - [Clonar o Repositório](#clonar-o-repositório)
    - [Executar com Docker Compose](#executar-com-docker-compose)
      - [🐳 Docker (docker-compose)](#-docker-docker-compose)
    - [Executar com Kubernetes](#executar-com-kubernetes)
      - [☸️ Kubernetes](#️-kubernetes)
  - [📚 Documentações](#-documentações)
  - [👨‍💻 Autores](#-autores)
  - [🔗 Repositórios de Microserviços](#-repositórios-de-microserviços)

---

# 📌 Aplicação Framepack-WebApi (API)

## 📖 Visão Geral
O Framepack-WebApi é uma API responsável pelo gerenciamento do fluxo de processamento de vídeos. Ela permite que usuários façam o upload de arquivos .MP4 para o Amazon S3 e gera eventos no Amazon SQS para que o serviço Framepack-Worker processe os vídeos.

A API também possibilita a consulta de uploads, visualização do status do processamento e download dos arquivos processados. Além disso, fornece funcionalidades de gestão de acessos, incluindo cadastro de usuários, login, redefinição de senha e confirmação de cadastro.

### Fluxo de Processamento
1️⃣ O usuário faz upload de um vídeo na Framepack-WebApi.
2️⃣ O vídeo é armazenado no Amazon S3 e um evento é enviado ao Amazon SQS.
3️⃣ O Framepack-Worker processa o vídeo (download, extração de frames, compactação e upload do arquivo final no S3).
4️⃣ O usuário pode consultar o status do processamento e realizar o download do arquivo final.
5️⃣ O sistema notifica o usuário por e-mail sobre o status da operação.

---

## 🚀 Funcionalidades Principais

#### Upload de Vídeos
O usuário pode enviar vídeos para o Amazon S3 via API.

#### Geração de Evento
Após o upload, a API gera um evento no Amazon SQS, permitindo que o Framepack-Worker inicie o processamento do vídeo.

#### Download de Vídeos
Permite o download dos vídeos processados, recuperando a URL armazenada no Amazon DynamoDB.

#### Consultas de Status
Os usuários podem visualizar a lista de uploads realizados e o status do processamento.

#### Gestão de Acessos
Gerenciamento de usuários, incluindo cadastro, login, redefinição de senha e confirmação de cadastro, utilizando o Amazon Cognito para autenticação e autorização.

---

## 📁 Estrutura do Projeto

A arquitetura do Framepack-WebApi segue uma abordagem modular e escalável, facilitando a manutenção e a evolução da aplicação.

- **🛠 BuildingBlocks** → Serviços e utilitários comuns (ex.: integração com Amazon S3, SQS).
- **📡 Controllers** → Controladores responsáveis pelo gerenciamento de requisições HTTP.
- **⚙️ DevOps** → Scripts e configurações para Docker e Kubernetes.
- **🎬 Gateways** → Handlers especializados na comunicação entre APIs e serviços.
- **🏗 Infra** → Configurações de banco de dados, serviços externos e infraestrutura necessária.
- **📌 UseCases** → Implementação dos principais casos de uso da API.
- **🖥 WebApi** → Contém a API principal do projeto.

---

## 🛠 Tecnologias Utilizadas

- **.NET 8** → Framework principal para desenvolvimento do backend.
- **Docker** → Containerização da aplicação, garantindo portabilidade e facilidade de deploy.
- **Kubernetes** → Orquestração de containers para resiliência e escalabilidade.
- **Amazon DynamoDB** → Banco de dados NoSQL para armazenamento de metadados.
- **Amazon SQS** → Serviço de mensageria para eventos assíncronos.
- **Amazon S3** → Armazenamento eficiente para vídeos e arquivos processados.
- **Amazon Cognito** → Gerenciamento de autenticação e autorização dos usuários.
- **CI/CD Automatizado** → Pipeline de integração e entrega contínua via GitHub Actions.
- **Análise de Código** → Qualidade do código garantida por SonarQube.

---

## ▶️ Como Executar o Projeto (Framepack-WebApi)

### Clonar o Repositório
```sh
git clone https://github.com/SofArc6Soat/framepack-api-hackathon.git
```

---

### Executar com Docker Compose
#### 🐳 Docker (docker-compose)
1️⃣ **Navegue até o diretório do projeto:**
```sh
cd framepack-api-hackathon/src/DevOps
```
2️⃣ **Configure o ambiente Docker:**
```sh
docker-compose up --build
```
3️⃣ **Acesse a aplicação:**
- API disponível em: `http://localhost:5001`
- Swagger: `http://localhost:5001/swagger`
- Healthcheck: `http://localhost:5001/health`

---

### Executar com Kubernetes
#### ☸️ Kubernetes
1️⃣ **Crie um arquivo `.env`** e configure as variáveis:
```plaintext
AWS_ACCESS_KEY_ID=your_access_key_id
AWS_SECRET_ACCESS_KEY=your_secret_access_key
AWS_REGION=your_region
```
2️⃣ **Aplique os arquivos YAML:**
```sh
kubectl apply -f 04-framepack-api-deployment.yaml
kubectl apply -f 05-framepack-api-service.yaml
kubectl apply -f 06-framepack-api-hpa.yaml
```
3️⃣ **Configure o port-forwarding para acessar a API:**
```sh
kubectl port-forward svc/framepack-api-service 8080:80 8443:443
```

---

## 📚 Documentações
Para acessar arquitetura, Domain Storytelling, Context Map, Linguagem Ubíqua, Event Storming e vídeos de demonstração, consulte o repositório de documentação:

🔗 **[Framepack-Doc-Hackathon](https://github.com/SofArc6Soat/framepack-doc-hackathon)**

---

## 👨‍💻 Autores

- **Anderson Lopez de Andrade** - RM: 350452
- **Henrique Alonso Vicente** - RM: 354583

---

## 🔗 Repositórios de Microserviços

- **Framepack-WebApi** → [🔗 GitHub](https://github.com/SofArc6Soat/framepack-api-hackathon)
- **Framepack-Worker** → [🔗 GitHub](https://github.com/SofArc6Soat/framepack-worker-hackathon)
- **Documentações** → [🔗 GitHub](https://github.com/SofArc6Soat/framepack-doc-hackathon)
