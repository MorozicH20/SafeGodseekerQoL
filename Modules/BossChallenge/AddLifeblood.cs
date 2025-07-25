namespace SafeGodseekerQoL.Modules.BossChallenge;

public sealed class AddLifeblood : Module {
	[GlobalSetting]
	[IntOption(0, 35, OptionType.Slider)]
	public static int lifebloodAmount = 0;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private static IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence) {
			yield break;
		}

		Add();
	}

	internal static void Add() {
		FixBlueHealthFSM();

		for (int i = 0; i < lifebloodAmount; i++) {
			EventRegister.SendEvent("ADD BLUE HEALTH");
		}

		LogDebug("Lifeblood added");
	}

	// Fix for Toggleable Bindings Shell Binding bug.
	private static void FixBlueHealthFSM() {
		PlayMakerFSM fsm = Ref.GC.hudCanvas.Child("Health")!.LocateMyFSM("Blue Health Control");
		if (fsm.ActiveStateName == "Wait") {
			fsm.SendEvent("LAST HP ADDED");
		}
	}
}
