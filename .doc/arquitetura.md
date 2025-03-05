#Arquitetura do sistema e estratégias escolhidas

## Microsserviços e Event Drive Architecture
O sistema foi desenhado seguindo uma arquitetura de micorsserviços e orientada a eventos, devido a requisitos de desacoplamento e escalabilidade independentes entre os serviços de "controle de lançamentos" e "consolidado diário".
Os serviços, em particular o de consolidação diária, foram desenhandos pensando em escalabilidade horizontal.

##Escalabilidade e balanceamento de carga (não implementado)
Dado o requisito de que em dias de pico o serviço consolidado diário pode receber até 50 requisições por segundo, o sistema deve ser capaz de expandir sua capacidade para atender a 50 requisições por segundo (RPS) sem degradação significativa do desempenho, 
portanto neste caso faria sentido o uso de escalabilidade horizonal, com criação automática (ou não) de mais instâncias do serviço e utilização de balanceador de carga (ex: NGINX, AWS ELB) para direcionar as requisições.

##Estratégias de Resiliência (não implementado):
- Circuit breaker: Polly para interromper a chamadas ao serviço se ele estiver sobrecarregado, evitando cascata de falhas.
- Implementar políticas de retry com polly para requisições que falharam (ex: tentar novamente após 5 segundos).
- Filas de Buffer: Usar filas de mensagens (ex: Redis Streams) para armazenar requisições temporariamente, processando-as assim que o serviço estiver disponível.

## Clean Architecture
Levando em conta aspectos de manutenibilidade e evolução dos software, foi escolhido um design de solução que permita separar a lógica 
de negócios da aplicação das suas dependências externas, tendo clara separação entre as camadas. Com isso se torna possível um crescimento 
sustentável e organizado do código da aplicação. Outra opção que seria interessante, principalmente se a complexidade dos domínios fosse maior, seria Arquitetura Hexagonal / Ports and Adapters.

##CQRS
Apesar de que neste caso cada microsserviço tem seu próprio banco de dados, o CQRS já prepara o sistema para um cenário onde haja necessidade de otimização de desempenho dentro de um mesmo domínio e/ou serviço, podendo haver 
a necessidade estruturas (banco de dados ou tabelas) separadas e otimizada para leitura e gravação de dados. 


## Outbox Pattern (não implementado)
Apesar de não implementado devido a falta de tempo, a utilização do Outbox Pattern seria importante neste cenário dada a necessidade de garantir a consistência e a confiabilidade da comunicação entre os microsserviços.
Neste caso seria implementado no serviço de controle de lançamento a fim de garantir que mesmo após uma falha na publicação da mensagem na fila, o serviço continuaria tentando entregar essa mensagem em um background service até ter sucesso, 
através de uma tabela que controle a publicação dos eventos na fila. Em um cenário diferente deste onde a consistência não fosse tão relevante, como por exemplo um serviço de notificação, essa abordagem talvez não fosse necessária e
a mensagem pudesse ser enviada diretamente para o destino sem validações de consistência.

#Cache distruído com Redis
Dado o requisito de grande número de acessos simultâneos ao serviço de consolidado diário, foi decidido pela utilização de cache distruído, já que num cenário com escalabilidade horizontal 
é esperado que as várias instância do mesmo serviço compartilhem o cache. Neste caso a primeira consulta é feita no banco de dados (Postgre) e em seguida armazenada no cache com data de expiração de 1 hora. 
Quando há atualização desse mesmo registro ele é removido do cache e a próxima consulta será novamente feita no banco de dados para obter os dados atualizados.


## Filas - Utilização de Redis Streams ao invés de RabbitMq ou Kakfa
Dado já existe uma instância do Redis sendo utilizada na estratégia de cache distribuído, e para evitar a utilização desnecessária de mais um serviço apenas para gerenciamento de filas, foi aproveitada a funcionalidade do Redis Streams para criação filas persistentes com suporte 
a grupos de consumidores e confirmação de mensagens (ACK). Os grupos de consumidores garantem o processamento distribuído de mensagens sem duplicação. Levando em conta que o serviço de consolidado diário poderá escalar horizontalmente, 
portanto tendo várias instâncias, então o balanceamento de Carga do redis garantiria que cada instância do serviço seria um consumidor distinto dentro do mesmo grupo, com o redis distribuindo as mensagens entre os consumidores 
do grupo e garantindo que apenas uma instância processe cada mensagem. Em outras palavras isso evita processamento duplicado de mensagens, pois utilizando grupo de consumidores o Redis mantém um registro de quais mensagens 
já foram entregues (pending messages) e quais foram confirmadas (ACK). 

##Health Check(não implementado): 
Numa aplicação financeira onde disponibilidade e resiliência são pontos críticos, o monitoramento da saúde da aplicação e suas dependências (banco de dados, filas, serviços externos etc) é indispensável. 
Em um cenário de aplicações distribuídas (Kubernetes, por exmeplo), são comuns estratégias de recuperação automática de aplicações ou dependências externas e até balanceamento de carga para instâncias que estejam saudáveis.


##Monitoramento / Observabilidade (não implementado): 
- Serilog: Logs centralizados e rastreamento, permitindo a estruturação e o envio de logs para diversos destinos, como bancos de dados, serviços de logging etc.
- ELK Stack (Elasticsearch, Logstash, Kibana e APM Server): Elasticsearch para armazenar e indexar os logs, Logstash para coleta e processamento dos logs, APM Server como ponto de entrada de dados de desempenho da aplicação, e Kibana para visualização. 
Outras opções como DataDog, Prometheus e Grafana também são interessantes.