# Security Assessment

## Identifying assets at risk

For large scale software and/or hardware systems, there are normally a lot of areas and assets at risk. However, since we are only doing a small scale system, we only really have two major assets that are at risk of cyber attacks. Our web application is running in a droplet at Digital Ocean, and is the main way of users communicating with our system. Here systems experience risks of attacks such as Injection attacks, Insecure Direct Object References (IDOR) and many more. Our database is running in a separate droplet at Digital Ocean, and is also open to a lot of risks. 

## Identifying possible threat sources

Our system is in no way perfect, and therefore has some areas that are open for cyber attacks. Injection attacks are one of the more common cyber attacks, and are possible for both SQL and NoSQL databases. In our case, when we a user wants to post a message, their message simply gets sent to the database without validating their message. This could let a potential hacker gain easy access to gaining private information about users. Another threat could be a potential Denial-of-Service attack (DDoS). This is something that is hard to prevent, and therefore is just something that we hope does not happen. Lastly, we also have some threats that we accidentally introduced ourselves. These threats include not hashing the passwords generated through hour SIM-api, and only doing it through our normal controller, and another threat is that our database connection string (along with the password) is in plain text in our GitHub repository. This means that anyone that knows of the existence of our repository will also have access to our database, leading to potential data leaks.

## Constructing Risk Scenarios

Throughout the rest of this section of the report, the following risk scenarios will be referred to by the number that correlates to each scenario: 

1. An attacker uses a DDoS’ing tool such as Low Orbit Ion Cannon to flood our systems bandwidth, thus causing a major user request delay in the best case scenario, or crashes or service all together. 
2. An attacker uses a NoSQL injection attack to get data on a user. This is possible as we do not validate input resulting in the attacker having easy access to our database
3. An attacker gets a hold of a password that has not been hashed by curling users, and thereby gains access to that user profile. This means the attacker can take over the identity of the user, thus gaining access to all the connections the user has.
4. An unwelcome party gets a hold of our GitHub repository, and thereby gets access to the connection string to our database. From here, the attacker can make a database dump, then delete the database, and lastly leave a readme file with instructions on how to pay him to get the database back. (we have experienced this)

## Determining the likelihood and risk of each scenario

Both likelihood and risk will be graded on a scale from low-very high

### Scenario 1:

DDoS attacks are normally not the go-to cyber attack, and therefore we have given it a likelihood rating of ‘low’. In some cases DDoS attacks only slow down the response time of the service as there is a lot of traffic, and user requests might take some time to get through that traffic. However, DDoS attacks can be very harmful if the attack is big enough resulting in the service’s bandwidth getting completed flooded resulting in the service running out of memory and crashing. As this scenario could potentially lead to our entire system failing, we have given this scenario a risk rating of “high/very high”.

### Scenario 2:

Injection attacks are in general one of the more common malicious attacks. Therefore, we have given these types of attacks a likelihood rating of “moderate”. Injection attacks would allow hackers to potentially get a hold of a lot of data regarding our users. As this project is only concerned with fake accounts and not actual people, this does not pose that much of a threat in our case. However, we are regarding all of the fake users as actual people, and therefore these types of attacks pose a large risk to the security and safety of our users. Therefore, we have decided to give this scenario a risk rating of “high”

### Scenario 3:

At the moment, our system is not very well protected as we are not hashing the passwords of the users created through our Simulation API. This means that anyone with a basic understanding of how web applications work, as well as a basic understanding of how to use a terminal, would be able to make a simple request, and get all of the users along with their passwords. Due to the low requirements a hacker needs to achieve this, we have decided to give this a likelihood rating of “high”. Most people with basic software knowledge would be able to do this as long as they have the domain that the server is running on. Due to this type of attack also regards personal user data, we have decided to give it a risk factor of “high” as well.

### Scenario 4:

Our database contains all of the information regarding our users. This means, that anyone that can access our database, will also have all of our user information. Furthermore, since we are using a public GitHub repository along with having our connection string (password included) in that repository, we have decided to give this attack a likelihood of “high”. Normally, you would have your database hidden away, but as this is a school exercise, as well as our first time dealing with cloud services, this is not something that we have done at the moment. Since this type of attack already happened to us, as well as having a big implication on our system, we have decided to give this type of attack a risk rating of “high/very high”

## Prioritising our scenarios using a risk-matrix

### Risk x Likelihood

|           |  Low  | Moderate  |  High  | Very High |
|-----------|-------|-----------|--------|-----------|
|    Low    | GREEN |   GREEN   | YELLOW |   ORANGE  |
|  Moderate | GREEN |   YELLOW  | ORANGE |    RED    |
|    High   | GREEN |   YELLOW  |   RED  |    RED    |
| Very High | GREEN |   ORANGE  |   RED  |    RED    |


### Assessing the scenarios

**Scenario 1**: LHR: Low, RR: High/Very High = Yellow/Orange

**Scenario 2**: LHR: Moderate, RR: High = Yellow

**Scenario 3**: LHR: High, RR: High = Red

**Scenario 4**: LHR: High, RR: High/Very high = Red

### Prioritising the scenarios

1. Scenario 4 - Unwanted database access
2. Scenario 3 - Easily accessible passwords
3. Scenario 1 - Distributed Denial of Service attacks
4. Scenario 2 - Injection attacks (NoSQL)

## Exploring possible solutions to each scenario

### Unwanted database access (connection string out in the open)

As mentioned in the header, the cause of this scenario is that our database connection string along with the password are openly accessible to anyone that knows of the existence of our GitHub repository. This means, to solve this issue, we simply have to find a way to hide that connection string. A possible solution to this would be encrypting the connection string, ensuring that even if people can get a hold of the connection string, it would be useless to them. 

### Easily accessible passwords

Unfortunately, there is not a lot we can do about the users that have already been added to the database. However, we do have a possible solution for the future. We already made a method for hashing passwords, and used it for our normal post api. However, we forgot to introduce it into our Simulation api, and therefore the passwords have not been hashed. 

### Distributed Denial of Service attacks

There is honestly not a lot we can do here. Bad DDoS scripts will simply be blocked by the firewall, however if a hacker really wants to take us down, and owns a good DDoS script, there would not be a whole lot we could do. Cloud providers also have some protection against DoS attacks, however these are not foolproof. In other words, we are passing this one on to our provider: DigitalOcean

### Injection attacks

Injection attacks are actually one of the easier attacks to prevent. Treat all user input as untrusted and validate their input. There are many ways of doing this, one of which is not using certain operators, as these can prove harmful (the operators vary from service to service). Another way is using sanitisation libraries (these also vary from service to service).