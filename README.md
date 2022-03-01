# dotNet5782_7227_3024

This project is part of "mini project in windows systems". 
In this project we develop delivery company by drones.

The project implements the 3-layers model in C#.NET.

The presentation layer implemented in WPF. We build two user interfaces, one for the customer and one for the managers.
The customer can see his parcels and add new parcels to send.

The manager (admin) can see the lists of all drones in the company, all base-stations, customers, amd parcels list.
In each list he can choose one item in the list and display his extend details.

Also, we implements the DAL twice. In one implementation we store all the data in lists, therefore all the details delete every time we close the project.
The second implementation we save all the data in xml files to make sure we don't lose any data every time we close the project.

We implement the Factory design pattern to control on those two implemetation, and to switch between the implementation you just need to change the 3-rd line in 'dal-config.xml' to the relevant elements there.

In the project we also create a simulator to simulate a life-sycle of one drone in the company. To turn on the simulator you first need to:
1. Login as admin.
2. Select 'Drone list'.
3. Select one drone. (duble click)
4. Click on the 'simulator' button.

The simulator work with BackgroundWorker. 

For fluent use in the xml DAL implementation, we recommend you to first click on the 'reset' button on the main window and restart the project.

The user names and paswords:
for customers: usernames - customer0, customer1, etc. and the passwords is the same as the usernames.
for managers its: admin for the username and password.

The VIP button is for login without username and password (for debugging uses).

Hope you will like this.
