
Static Game Data
    - We inject static data as a dependancy so that if the cache expires mid-request then the data is retained
        instead of potentially having two different versions of the data