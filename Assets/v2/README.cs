
/*
    __Data Types__
        - PersistantLocalFile
            - Data which is stored locally but will persist between prestiges
        - StateLocalFile
            - Data which is relevant to the current prestige and will be deleted
        - StaticData
            - Game values pulled from the server or scriptable objects
        - PersistantUserData (maybe?)
            - Data pulled from the server. The C# class will most likely be updated using the PersistantLocalFile 
 
    __Model Classes__
        - Models are primarily used as data transfer objects between either the server or file system        
        - Model classes can contain other attributes which are set at runtime (ex. values from scriptable objects) but once a model 'requires' a method to be implemented,
            then a new class should be created and all logic should move away from the model class (ex. model classes should contain attributes only)
*/