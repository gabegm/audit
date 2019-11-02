# audit
c# coding exercise

Implement a Console Application using C#
Application should make an HTTP GET request to the following endpoint URL; http://jsonplaceholder.typicode.com/photos which will respond with a list of images stored on a mock database.
Parse the result JSON data and store the list of database rows on a data structure you see fit. (Explain your decision on the data structure with comment blocks)
Sort the rows by their title field lexicographically and write the sorted result set into a CSV file named results.csv on user’s Desktop. Columns in csv file should be ID, ALBUMID, TITLE, URL, THUMBNAILURL respectively. (Tip: username for the Desktop directory location will change according to the logged in user’s username so don’t use any static usernames)
Using your data structure and an algorithm that has acceptable computational complexity, compute a top ten list for words that are recurring the most on title fields of the images. Print out the top ten list on console before your application waits for a user response to terminate execution.
Comment out your code in detail and log your application steps to console in each step.
