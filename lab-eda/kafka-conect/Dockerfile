ARG STRIMZI_VERSION=latest-kafka-3.4.0
FROM quay.io/strimzi/kafka:${STRIMZI_VERSION}
USER root:root
RUN mkdir -p /opt/kafka/plugins
COPY ./plugin/ /opt/kafka/plugins/
USER 1001
