package nudemeth.poc.ordering.api.application.query.viewmodel

import java.time.OffsetDateTime
import java.util.UUID

case class Order(
  orderNumber: UUID,
  date: OffsetDateTime,
  status: String,
  description: String,
  street: String,
  city: String,
  zipCode: String,
  country: String,
  orderItems: Vector[OrderItem],
  total: Double) {

}
