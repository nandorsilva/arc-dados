# Projeto Final Event Driven

## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


## Pré-requisitos?
* Docker
* Docker-Compose


![Projeto final](/content/projeto-final.png)

## Contexto

Uma empresa de comércio eletrônico quer garantir que os pedidos, itens do pedido e informações de produtos estejam sempre atualizados em seus sistemas internos e em um banco de dados de análise em tempo real. Eles optaram por uma abordagem event-driven para garantir a sincronização eficiente entre diferentes sistemas e entre seu banco de dados transacional com NoSQL.

## Componentes da Arquitetura:

### Banco de Dados SQL (Fonte de Dados):

Armazena informações sobre `pedidos`, `itens do pedido` e `produtos`.
Tabelas: `pedidos`, `itens_pedido`, `produtos`.




### Kafka Connect - Debezium para SQL:

O Kafka Connect Debezium é configurado para monitorar essas tabelas e emitir eventos quando houver inserções, atualizações ou exclusões, usando CDC.


### Apache Kafka (Barramento de eventos):

Recebe os eventos/mensagens de alteração ou inclusões de dados pelo conector Debezium.
Utiliza tópicos do Kafka para segmentar e distribuir os eventos.


### Kafka Connect - Debezium para MongoDB:

Captura eventos transformados pelo KSqldb relacionados a pedidos, itens do pedido e produtos.
Transforma esses eventos em eventos adequados para o MongoDB.
Utiliza um conector Debezium especializado para MongoDB para enviar os eventos transformados para os tópicos do Kafka associados ao MongoDB.

### Kafka Streams/KSQL (Processamento de Eventos):

Lê eventos do tópico do Kafka vinculado ao banco de dados SQL Server, banco de dados transacional.
Realiza processamento em tempo real usando KSqldb para enriquecer os dados, como agregar informações `pedidos`, `itens_pedido`, `produtos`, calcular totais, ou formatar informações.


### Banco de Dados MongoDB (Destino dos Dados Processados):

Armazena em documentos os `tópicos` enriquecidos ou processados após o processamento no Ksqldb.

Utiliza o conector Debezium para MongoDB para salvar as informações.

Estrutura de Dados: `Pedido`, `TotalPedido`.


## Benefícios:

Sincronização em Tempo Real: As alterações nos pedidos, itens do pedido e produtos no banco de dados SQL são capturadas e sincronizadas em tempo real com o MongoDB.
Análise Avançada: O processamento em tempo real permite a análise avançada dos pedidos, como calcular totais, e outras métricas relevantes.
Consistência de Dados: Todos os sistemas têm acesso às mesmas informações atualizadas, eliminando possíveis discrepâncias entre os sistemas.
Escalabilidade: A arquitetura é escalável, permitindo a adição de mais recursos conforme a demanda aumenta.

## Resultado:
A empresa de comércio eletrônico consegue manter os sistemas internos e o banco de dados de análise em tempo real sempre atualizados com informações de pedidos, itens do pedido e produtos. Isso facilita o acompanhamento preciso das operações e permite análises avançadas para aprimorar a tomada de decisões e a experiência do cliente.