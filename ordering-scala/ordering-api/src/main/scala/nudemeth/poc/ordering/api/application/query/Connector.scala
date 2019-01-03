package nudemeth.poc.ordering.api.application.query

import com.outworkers.phantom.connectors.{ CassandraConnection, ContactPoints }
import com.typesafe.config.ConfigFactory

import scala.collection.JavaConverters._

object Connector {
  private val config = ConfigFactory.load()
  private val hosts = config.getStringList("cassandra.host").asScala
  private val keyspace = config.getString("cassandra.keyspace")

  lazy val connector: CassandraConnection = ContactPoints(hosts).keySpace(keyspace)
}
