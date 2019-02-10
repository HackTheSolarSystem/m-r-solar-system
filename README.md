# SolAR

[ Mixed Reality Multi-User exploration of the solar system ](https://github.com/amnh/HackTheSolarSystem/wiki/A-Mixed-Reality-Solar-System)

### Created by Solar Squad 

| Name            | Github Handle |
| :-------------: |:-------------:|
| Anish Parikh | anish2591 |
| Claire Samuels | clairemation |
| Shan Liu | oranliu |
| Nishant Mehta | nishantmehta |
| Kate Borst | kborst83 |
| Lenn Hypolite | lennhy |
| Anthonia Carter | anthoniaocc |
| Stephanie Tsuei | stsuei88 |

# Solution Description

### Project Goal
Our project is an immersive learning experience where one to as many people can visualize the grand scale of our solar system and zoom into the planetary level to learn about properties such as atmosphere, mass, core, and distance for each planet.

### Implementation
To achieve this we leveraged the solar system model in unity which was provided to us. We built on top of this and created a higher resolution model for each planet with more details about it like its atmosphere, core etc. We then connected the unity models to a backend express server which renders facts and questions about each planet in a JSON format. The server here can take in a simple csv file as an input or connect to a database/engine which can provide it with the facts and questions.

![Workflow](https://github.com/HackTheSolarSystem/m-r-solar-system/blob/master/images/Image%20from%20iOS.jpg "Workflow")

The users would see the facts and questions on the device which they are using to interact with the models. They would be able to answer the questions with the help of the facts provided and the answers would be validated by the server. 

### Future work 
* For future work, we can implement a system which can keep score for each user and at the end of the session provide details and statistics about each user who was part of that session 
* The server component is modular enough to easily take in any other dataset like marine life, geographical data etc.

# Installation Instructions
* Express Node.js server
* Unity


You must also provide any step-by-step instructions for installation of your solution.

Step one - install package manager
Step two - special config instructions
Step three - system administration notes
Step four - command line how-to, listing descriptions of all optional arguments
