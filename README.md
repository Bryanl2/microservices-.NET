# microservices-.NET
This solution was developed under a microservice's architecture. It consists of two differents solutions making contact each other through a service bus , specifically RabbitMQ.
Technologies:

-Docker
-Kubernetes
-.NET6
-RabbitMQ

The main purpose of the project is the creation of differents platforms (first solution) are going to be read by the infrastructure team (second solution). The project was designed under a microservice domain with 1 cluser, 4 nodes, 4 pods (two pods are occupied by our services, third one Microsoft server database and last one by the service bus), 1 gateway.
