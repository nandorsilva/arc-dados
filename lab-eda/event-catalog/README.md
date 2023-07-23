# Lab Eda - Catalogo de Eventos


## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 


## Pré-requisitos?
* Docker
* Docker-Compose
* Node.js 16.14.0 and higher
* Yarn 1.5 and higher

> https://classic.yarnpkg.com/en/


# Instalação Kafka 

[LAB EDA](lab-eda//README.md)


# Criando o Catalog Event


```
npx @eventcatalog/create-eventcatalog@latest catalogo-fia

cd catalogo-fia
npm run dev

```


## Instalando o plugin AsyncAPI

```
npm install --save @eventcatalog/plugin-doc-generator-asyncapi
```

Procura o arquivo ´eventcatalog.config.js´ e add o codigo abaixo:


```
const path = require('path');

module.exports = {
   generators: [
    [
      '@eventcatalog/plugin-doc-generator-asyncapi',
      {
        // path to your AsyncAPI files
        pathToSpec: [path.join(__dirname, 'asyncapi.yml')],

        // version events if already in catalog (optional)
        versionEvents: true
      },
    ],
  ],
};

```

Gerando as alterações

```
npm run generate
cd my-catalog
npm run dev
```


https://www.eventcatalog.dev/docs/guides/deployment