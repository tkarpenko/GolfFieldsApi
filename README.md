# Golf Fields Api

The source code is the implamentation of a pod of the [microservice application](https://github.com/tkarpenko/MicroserviceWithKubernetes).


Azure DevOps CI

![ci](https://github.com/tkarpenko/GolfFieldsApi/blob/main/Azure%20DevOps%20CI.jpg)

Azure DevOps CD

![cd](https://github.com/tkarpenko/GolfFieldsApi/blob/main/Azure%20DevOps%20CD.jpg)


To enrich API endpoints do the following:
* Install docker on your machine
* ```> git clone https://github.com/tkarpenko/MicroserviceWithKubernetes.git```
* ```> cd MicroserviceWithKubernetes```
* ```> sh Install.sh```
* ```> kubectl get pods --namespace golf-fields-api```
* wait till a pod will have status running

* then run a command
* ```> kubectl port-forward service/golf-fields-api-service --namespace golf-fields-api 5000:5000```
* open Postman
* do POST request as on image below
![postman](https://github.com/tkarpenko/GolfFieldsApi/blob/main/Postman.jpg)
