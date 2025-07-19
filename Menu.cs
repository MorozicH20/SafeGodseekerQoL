using Satchel.BetterMenus;

using MenuButton = Satchel.BetterMenus.MenuButton;

namespace SafeGodseekerQoL;

public sealed partial class SafeGodseekerQoL : ICustomMenuMod {
	bool ICustomMenuMod.ToggleButtonInsideMenu => true;

	public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) =>
		ModMenu.GetMenuScreen(modListMenu, toggleDelegates);

	private static class ModMenu {
		private static bool dirty = true;
		private static Menu? menu = null;

		static ModMenu() => On.Language.Language.DoSwitch += (orig, self) => {
			dirty = true;
			orig(self);
		};

		internal static MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			if (menu != null && !dirty) {
				return menu.GetMenuScreen(modListMenu);
			}
            // Создаем список элементов меню
            List<Element> menuElements = new();

            // Добавляем переключатель мода, только если toggleDelegates не null
            if (toggleDelegates.HasValue)
            {
                menuElements.Add(toggleDelegates.Value.CreateToggle(
                    "ModName",
                    "ToggleButtonDesc"
                ));
            }
            menu = new("ModName", [.. menuElements]);

   //         menu = new("ModName", [
			//	toggleDelegates!.Value.CreateToggle(
			//		"ModName",
			//		"ToggleButtonDesc"
			//	)
			//]);

			ModuleManager
				.Modules
				.Values
				.Filter(module => !module.Hidden)
				.GroupBy(module => module.Category)
				.OrderBy(group => group.Key == nameof(Modules.Misc))
				.ThenBy(group => group.Key)
				.Map(group => Blueprints.NavigateToMenu(
					$"Categories/{group.Key}",
					"",
					() => new Menu(
						$"Categories/{group.Key}",
						[
							..group.Map(module => Blueprints.HorizontalBoolOption(
								$"Modules/{module.Name}",
								module.Suppressed
									? string.Format(
										"Suppression",
										module.suppressorMap.Values.Distinct().Join(", ")
									)
									: $"ToggleableLevel/{module.ToggleableLevel}",
								(val) => module.Enabled = val,
								() => module.Enabled
							)),
							..Setting.Global.GetMenuElements(group.Key),
						]).GetMenuScreen(menu.menuScreen)
				))
				.ForEach(menu.AddElement);

			menu.AddElement(new MenuButton(
				"ResetModules",
				string.Empty,
				btn => ModuleManager.Modules.Values.ForEach(
					module => module.Enabled = module.DefaultEnabled
				),
				true
			));

			dirty = false;
			return menu.GetMenuScreen(modListMenu);
		}

	}
}
