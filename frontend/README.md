
## Stack utilizada

| ![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white) | ![Vue.js](https://img.shields.io/badge/vuejs-%2335495e.svg?style=for-the-badge&logo=vuedotjs&logoColor=%234FC08D) |
|--------------|--|
| ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white) | ![Vuetify](https://img.shields.io/badge/Vuetify-1867C0?style=for-the-badge&logo=vuetify&logoColor=AEDDFF) |

## Pré-requisitos

- [Node](https://nodejs.org/pt)
- [Backend](https://github.com/Elian-beep/FabrilOS/tree/main)

## Instalação e execução

Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/FabrilOS.git
cd FabrilOS
```

Execução via Docker
```bash
docker-compose docker-compose.yml up -d --build
```

Execução local
```bash
cd frontend
npm install
npm run dev
```

Obs: A execução local esta configurada na porta 5173 e o container na porta 8080

## Trabalhos futuros

- Tela 404 padrão
- Registro de usuário ao lado do login
- Interceptor para limpar token caso ocorra um 401

