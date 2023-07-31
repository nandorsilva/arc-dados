# Lab Eda


### Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


### Pré-requisitos?
* Kubernetes



## Instalação Kubernets Kafka 



## Inserindo o operator Strimzi
```
helm repo add strimzi https://strimzi.io/charts/
```

## Instalando o operator Strimzi

```
helm install strimzi-kafka strimzi/strimzi-kafka-operator
```

## Sudindo o cluster kafka 
```
cd kafka
kubectl apply -f kafka.yaml
kubectl get kafka
```

https://strimzi.io/
https://operatorhub.io/operator/strimzi-kafka-operator


## Deletando o cluster kafka 
kubectl delete -f kafka.yaml