package nudemeth.poc.ordering.api.infrastructure.mediator

import scala.concurrent.Future

class Mediator(handlers: Map[Class[_ <: Request[Any]], _ <: RequestHandler[_ <: Request[Any], Any]]) extends MediatorDuty {
  override def send[TResponse](request: Request[TResponse]): Future[TResponse] = {
    val handler = handlers.find(h => h._1.equals(request.getClass)).getOrElse(throw new NoSuchElementException(classOf[Request[TResponse]].toString))
    handler.asInstanceOf[RequestHandler[Request[TResponse], TResponse]].handle(request, this)
  }
}