package nudemeth.poc.ordering.infrastructure.repository

import java.util.UUID

import nudemeth.poc.ordering.domain.model.aggregate.order.Order

import scala.concurrent.Future

trait OrderRepositoryOperations {
  def getOrderAsync(id: UUID): Future[Option[Order]]
  def addOrderAsync(order: Order): Future[Order]
  def updateOrderAsync(order: Order): Future[Unit]
}
