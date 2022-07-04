
/*
    __Data Types__
        - PersistantLocalFile
            - Data which is stored locally but will persist between prestiges
        - StateLocalFile
            - Data which is relevant to the current prestige and will be deleted
        - StaticData
            - Game values pulled from the server or scriptable objects
        - UserData
            - Data pulled from the server
 
    __Model Classes__
        - Models are primarily used as data transfer objects between either the server or file system        
        - Model classes can contain other attributes which are set at runtime (ex. values from scriptable objects) but once a model 'requires' a method to be implemented,
            then a new class should be created and all logic should move away from the model class (ex. model classes should contain attributes only)
    
    __Popup vs Panel vs Modal vs Alert__
        - Generally will be 'Panel' for sake of consitency
        - Modal may be 'ConfirmModal' or 'QuantityModal' etc. Some kind of input
*/