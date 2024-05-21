ARG STRIMZI_VERSION=latest-kafka-3.7.0

# Cria uma camada temporária para instalar unzip e extrair o arquivo usando uma imagem Debian
FROM debian:latest as unzip-layer

RUN apt-get update && apt-get install -y unzip curl

WORKDIR /tmp/connect-plugins

RUN curl -sfSL https://api.hub.confluent.io/api/plugins/confluentinc/kafka-connect-s3/versions/10.5.5/archive -o archive.zip &&\
    unzip archive.zip 


FROM quay.io/strimzi/kafka:${STRIMZI_VERSION} as imagemfinal

ARG DEBEZIUM_CONNECTOR_VERSION=2.6.1.Final
ENV KAFKA_CONNECT_PLUGIN_PATH=/tmp/connect-plugins/
ENV KAFKA_CONNECT_LIBS=/opt/kafka/libs

RUN mkdir $KAFKA_CONNECT_PLUGIN_PATH &&\
    cd $KAFKA_CONNECT_PLUGIN_PATH &&\
    curl -sfSL  https://repo1.maven.org/maven2/io/debezium/debezium-connector-postgres/${DEBEZIUM_CONNECTOR_VERSION}/debezium-connector-postgres-${DEBEZIUM_CONNECTOR_VERSION}-plugin.tar.gz | tar xz &&\
    cd debezium-connector-postgres &&\
    curl -sfSL https://repo1.maven.org/maven2/io/debezium/debezium-interceptor/${DEBEZIUM_CONNECTOR_VERSION}/debezium-interceptor-${DEBEZIUM_CONNECTOR_VERSION}.jar -o debezium-interceptor-${DEBEZIUM_CONNECTOR_VERSION}.jar

 COPY --from=unzip-layer /tmp/connect-plugins/confluentinc-kafka-connect-s3-10.5.5 ${KAFKA_CONNECT_PLUGIN_PATH}/confluentinc-kafka-connect-s3-10.5.5