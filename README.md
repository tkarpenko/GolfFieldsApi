# Golf Fields Api

The source code is the implamentation of a pod of the [microservice application](https://github.com/tkarpenko/MicroserviceWithKubernetes).

To enrich API endpoints do the following:
* Install docker on your machine
* ```> git clone https://github.com/tkarpenko/MicroserviceWithKubernetes.git```
* ```> cd MicroserviceWithKubernetes```
* ```> sh Install.sh```
* ```> kubectl get pods --namespace golf-fields-api```
* wait till a pod will have status running
* ```> kubectl port-forward service/golf-fields-api-service --namespace golf-fields-api 5000:5000```
* open Postman
* do POST request as on image below
