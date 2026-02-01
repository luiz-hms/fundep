# Desafio Técnico – FUNDEP

Este projeto foi desenvolvido como parte de um desafio técnico para a FUNDEP, com o objetivo de demonstrar conhecimentos básicos em ASP.NET Web Forms, organização de código, separação de responsabilidades e persistência de dados sem uso de banco de dados.

O sistema implementa o cadastro e consulta de projetos e coordenadores, seguindo os requisitos propostos no desafio.

⏱ **Tempo aproximado de desenvolvimento:** 15 horas

---

## 🧠 O que eu entendi sobre as tecnologias utilizadas

### ASP.NET Web Forms
O ASP.NET Web Forms é um framework da Microsoft para desenvolvimento de aplicações web baseado em eventos, onde a página funciona de forma semelhante a aplicações desktop.  
O desenvolvedor trabalha com componentes prontos como TextBox, Button, GridView, etc... e interage com eles através de eventos no servidor, como `Click` e `Load`.

Apesar de ser uma tecnologia mais antiga, ainda é bastante utilizada em sistemas legados e exige atenção especial ao ciclo de vida da página e aos postbacks.

---

### Code-behind
O code-behind é a separação entre o HTML (ASPX) e a lógica de negócio (arquivo `.aspx.cs`).  
No arquivo `.aspx` ficam apenas os componentes visuais e suas configurações, enquanto no `.cs` ficam os métodos, validações, chamadas de serviços e regras de negócio.

Essa separação ajuda a manter o código mais organizado, legível e fácil de manter.

---

### WCF (Windows Communication Foundation)
O WCF é utilizado para expor serviços que podem ser consumidos por outras aplicações.  
Neste projeto, ele foi usado para encapsular toda a lógica de acesso e manipulação dos dados (projetos e coordenadores), evitando que a aplicação web acesse diretamente os arquivos XML.

O serviço foi implementado de forma simples, respeitando os contratos definidos por interfaces.

---

### Comunicação via DLL
O serviço WCF foi disponibilizado através de uma DLL, que é referenciada pelo projeto Web Forms.  
A aplicação web consome o serviço apenas por meio de contratos (interfaces), sem conhecer detalhes da implementação.

Essa abordagem segue um padrão semelhante ao Repository, promovendo desacoplamento, organização e facilitando futuras manutenções ou substituições da implementação.

---

## ⚙️ Principais decisões técnicas adotadas

- Utilização de **ASP.NET Web Forms (.NET Framework 4.8)**, conforme compatibilidade e requisitos do desafio.
- Persistência de dados em **arquivos XML**, evitando dependência de banco de dados.
- Criação de um **serviço WCF em DLL** para centralizar a lógica de negócio.
- Consumo do serviço no WebApp apenas via **interfaces (contratos)**.
- Separação clara entre:
  - Camada de apresentação (Web Forms)
  - Camada de serviço (WCF)
  - Modelos de domínio
- Validações tanto no **front-end** quanto no **back-end**.
- Uso de **UpdatePanel** para melhorar a experiência do usuário sem recarregar a página inteira.
- Padronização visual utilizando as **cores institucionais da FUNDEP**.
- Implementação de um **login simples**, apenas com validação de campos obrigatórios.

---

## 🚀 Sugestões de melhorias futuras

Algumas melhorias que poderiam ser implementadas em uma próxima etapa:

- 🔐 **Autenticação**, com usuários reais, perfis e controle de acesso.
- 💰 **Módulo financeiro**, permitindo registrar despesas dos projetos e atualizar automaticamente o saldo.
- 🗄 Substituição do XML por **banco de dados** (SQL Server ou outro).
- 📄 Paginação e ordenação mais avançada nas listagens.
- 🎨 Melhorias adicionais de UX, como feedbacks visuais mais detalhados.

---

## 📌 Observações finais

O foco deste projeto foi atender o mais fielmente possível aos requisitos do desafio, mantendo o código simples, organizado e fácil de entender.

Todo o desenvolvimento foi feito buscando clareza, boas práticas e espaço para evolução futura do sistema.
