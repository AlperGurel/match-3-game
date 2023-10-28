namespace  Match3
{
    
    public interface ISkill
    {
        public abstract void Initialize(IBaseSkillData baseSkillData);
        public abstract void SetItem(Item item);
    }

    public interface IBaseSkillData
    {
        public abstract string GetName();
    }
}
