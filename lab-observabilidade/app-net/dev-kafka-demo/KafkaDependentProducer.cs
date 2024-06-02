using Confluent.Kafka;

namespace dev_kafka_demo
{
    public class KafkaDependentProducer<K, V>
    {
        IProducer<K, V> kafkaHandle;

        public KafkaDependentProducer(KafkaClientHandle handle)
        {
            kafkaHandle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
        }

       
        public Task ProduceAsync(string topic, Message<K, V> message) => this.kafkaHandle.ProduceAsync(topic, message);

      
        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>> deliveryHandler = null)
            => this.kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => this.kafkaHandle.Flush(timeout);
    }
}
