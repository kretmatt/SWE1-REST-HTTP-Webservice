# SWE1-REST-HTTP-Webservice

Webservice handler based on a restful HTTP-based server. For testing purposes a message resource is created and all corresponding methods (CRUD) are implemented.

# Special explanatory notes

Further steps are explained in this section.

## Class Diagram

BaseHTTPServer receives the HTTP-Request an reads the from the NetworkStream. Afterwards all resource endpoint handlers are iterated and the "right" one is selected. Depending on the url structure and the method (GET, POST, PUT, DELETE) different CRUDHandler methods are used. Those CRUD handler methods do their "task" and send a response to the requester using the HTTPResponseHandler afterwards. 

![Class diagram of the project](./httprestserver_classdiagram/httprestserver_classdiagram.svg)


## Route Actions

I wasn't really sure how to implement different endpoints and check if they are valid. That's why i created the RouteAction class. It consists of the Type/method (e.g.: GET), a regex expression which is used for validating the requested url and an Action. The action is executed if the regex expression and the method equal the HTTP request.
