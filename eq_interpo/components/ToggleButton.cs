using ui.components;

namespace eq_interpo.components
{
    public class ToggleStore : ComponentStore
    {
        public bool isToggled = false;
    }

    public class ToggleButton : Button<ToggleStore, ToggleButton>
    {

        public ToggleButton(string text = null) : base(text)
        {

        }

        public override ToggleStore ComponentStoreConstructor()
        {
            return new ToggleStore();
        }

    }
}
