# Sistema para controle de lançamentos e consolidado diário

## Descrição funcional
O conjunto de Microsserviços visa atender a demanda de um comerciante que precisa controlar o seu fluxo de caixa diário com os lançamentos (débitos e créditos). Além disso precisa de um relatório que disponibilize o saldo diário consolidado.

**Serviço de Controle de Lançamentos**
- Registra os lançamentos de crédito e débito.
- Valida se a conta bancária existe e se tem saldo em caso de débito. 
- A cada lançamento o saldo da conta é atualizado (não confundir com consolidação diária).
- Estorno de lançamento (não implementado).
- Consulta com extrato de lançamentos (não implementado)
- Consulta de saldo da conta (não implementado)

**Serviço de consolidado diário**
 - A cada novo lançamento o serviço recalcula o saldo diário consolidado da conta. 
 - Consulta por data e conta: Exibe total de créditos, total de débitos, saldo consolidado, data de consolidação e data da última atualização do saldo.
 - Consulta de saldo consolidado por período (não implementado).


## Decisões arquiteturais
[Arquiteira do sistema](/.doc/arquitetura.md): Essa seção trata das decisões relacionadas a padrões arquiteturais e do design do sistema 

## Frameworks e serviços
[Frameworks](/.doc/frameworks.md): Essa seção lista os Framework e serviços utilizados no projeto

## Observações
- Para fins didáticos ambos os serviços estarão no mesmo repositório, mas em um cenário real cada serviço teria seu próprio repositório, com branchs develop, staging, release etc, seguinte as boas práticas do Git Flow.
- A arquitetura e o design escolhido foram pensados no contexto de um sistema com mais funcionalidades e evoluções futuras, portanto muito mais complexo. Requisitos como disponibilidade, escalabilidade e resiliência também pesaram nas decisões arquiteturais tomadas.