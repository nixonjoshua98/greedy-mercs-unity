namespace GM.Common.Interfaces
{
    interface ILocalStateFileSerializer
    {
        void UpdateLocalSaveFile(ref LocalSaveFileModel model);
    }
}
