package nudemeth.poc.ordering.api.controller

import java.util.UUID

import akka.actor.{ Actor, ActorLogging, Props }
import akka.pattern.pipe
import akka.util.Timeout

import scala.concurrent.duration._
import nudemeth.poc.ordering.api.application.query.OrderQueryable
import nudemeth.poc.ordering.api.infrastructure.service.IdentityService.UserIdentity

import scala.concurrent.ExecutionContext

object OrderingRegistryActor {
  final case class ActionPerformed(description: String)
  final case class GetOrders(userIdentity: UserIdentity)
  final case class GetCardTypes()
  final case class GetOrder(id: UUID)
  final case class CancelOrder(requestId: String)
  final case class ShipOrder(requestId: String)

  def props(repository: OrderQueryable): Props = Props(new OrderingRegistryActor(repository))
}

class OrderingRegistryActor(repository: OrderQueryable) extends Actor with ActorLogging {
  import nudemeth.poc.ordering.api.controller.OrderingRegistryActor._
  implicit val ec: ExecutionContext = context.dispatcher
  implicit val timeout: Timeout = 10.seconds

  def receive: Receive = {
    case GetOrders(userIdentity: UserIdentity) =>
      repository.getOrdersByUserIdAsync(userIdentity.id).pipeTo(sender())
    case GetOrder(id) =>
      repository.getOrderAsync(id).pipeTo(sender())
    case GetCardTypes() =>
      sender() ! ""
    case CancelOrder(requestId) =>
      sender() ! ""
    case ShipOrder(requestId) =>
      sender() ! ""
  }
}