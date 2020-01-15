package nudemeth.poc.ordering.infrastructure.eventbus

import java.time.Instant
import java.util.UUID

class IntegrationEvent() {
  val id: UUID = UUID.randomUUID()
  val creationDate: Instant = Instant.now()
}