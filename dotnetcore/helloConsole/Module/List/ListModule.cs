namespace helloConsole.Module.List
{
    public class ListModule
    {
        public ListModule()
        {

        }


        public void TestAddList(List<object> refLst, object newItem)
            => refLst.Add(newItem);

        public void TestAddListRange(List<object> refLst, List<object> newItems)
            => refLst.AddRange(newItems);
    }
}