# Lab Eda - Jaeger

## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 


## Pré-requisitos?
* Docker
* Docker-Compose


# Instalação Kafka 

[LAB EDA](lab-eda//README.md)

## Jaeger


O suporte de rastreamento é baseado em dois projetos de código aberto:

* OpenTracing
* Jaeger

Ambos os projetos fazem parte da Cloud Native Computing Foundation, https://www.cncf.io/.

> OpenTracing é uma API independente para rastreamento distribuído que oferece suporte a diferentes protocolos e linguagens de programação. O próprio OpenTracing não implementa o rastreamento. Ele fornece apenas uma API interoperável para várias linguagens de programação. Outros projetos usam essas APIs para construir a implementação de rastreamento real sobre ela, como por exemplo o Jaeger.

O cliente Jaeger é executado como parte de seu aplicativo. Por exemplo, produtor ou consumidor do Kafka, ou um dos componentes Kafka, como Kafka Connect ou Mirror Maker 2.0.

O aplicativo usa a API OpenTracing para criar extensões de rastreamento e anexar IDs de rastreamento a solicitações de saída. O cliente Jaeger é responsável por propagar as informações sobre os spans para o agente Jaeger. 


O cliente geralmente não envia todos os rastreamentos ao agente, mas geralmente apenas uma pequena porcentagem, uma amostragem.

## Instalando  Jaeger

```

docker-compose  -f ambiente/docker-compose.yaml -f jaeger/docker-compose-jaeger.yaml up -d zk kafka-broker jaeger sqlserver connect

```

http://localhost:16686/

> Configura um conector SQL no link Kafka Connect
3. [LAB EDA](../kafka-conect/README.md)



# A imagem jaeger contem:

* `Agente` é o componente localizado no aplicativo para reunir os dados de rastreamento Jaeger localmente. Ele lida com a conexão e o controle de tráfego para o Coletor, bem como o enriquecimento dos dados.

* `O Jaeger Coletor`  é o componente responsável por receber os spans que foram capturados pelos Agente e gravá-los em um armazenamento.

* `Jaeger Consult`a é um serviço que recupera rastros do armazenamento e hospeda a interface do usuário para exibi-los.

---

## Estratégias de implantação

`All-in-One`: Esta é uma configuração fácil de implantar, boa para experimentar o produto, desenvolvimento e uso de demonstração. Você pode executá-lo como um binário predefinido ou uma imagem Docker. `Opção default`.

`Produção`: focado nas necessidades do ambiente de produção para alta disponibilidade e escalabilidade. Ele implanta cada serviço de back-end de forma independente e oferece suporte a várias réplicas e opções de dimensionamento. Ele também usa armazenamento de back-end persistente para manter os dados de rastreamento resilientes. Atualmente, ele oferece suporte às soluções de armazenamento `Elasticsearch`, `Cassandra` e `kafka`, com Elasticsearch como a solução recomendada para ambientes de produção.

`Streaming`: para ambientes de alta carga, esta configuração adiciona Kafka à estratégia de implantação de produção para tirar a pressão do armazenamento de back-end. Se você precisar executar a lógica de pós-processamento nos rastreamentos, será mais fácil executar antes de gravar no armazenamento.


## Alterando o Kafka Connect

A alteração é simples basta informar o tracing e as variaveis de ambiente


A lista completa de variáveis ​​suportadas e seus significados podem ser encontrados em https://github.com/jaegertracing/jaeger-client-java/blob/master/jaeger-core/README.md#configuration-via-environment

As informações de rastreamento ficam no cabeçalho das mensagens, para obter mais informações, consulte a documentação do OpenTracing  como injetar (https://opentracing.io/docs/overview/inject-extract/) e extrair os ids de rastreamento. Eles podem usar o rastreador global criado e registrado por Strimzi para enviar os rastros para Jaeger.

*Mensagens com o rastreamento*

```

docker exec -it kafka-broker /bin/bash

kafka-topics --bootstrap-server localhost:9092 --list 

 kafka-console-consumer --bootstrap-server localhost:9092 --topic sqldebezium.dbo.produtos  --property print.timestamp=true --property print.key=true --property print.headers=true --property print.value=true --property print.partition=true --from-beginning

```


## Vamos criar mais um consumidor

```
docker-compose  -f ambiente/docker-compose.yaml -f jaeger/docker-compose-jaeger.yaml up -d  jaegerconsumer

```

## Produzindo mais mensagens

```
export SA_PASSWORD=Password!

docker exec -i sqlserver /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -d dbEcommerce -Q "INSERT INTO produtos(nome,descricao)  VALUES ('Lapis','lapis de escrever');"

```

## Mais um consumidor

```
docker-compose  -f ambiente/docker-compose.yaml -f jaeger/docker-compose-jaeger.yaml up -d  jaegerconsumer2

```