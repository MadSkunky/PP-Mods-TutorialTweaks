{
    Id: "MadSkunky.TutorialTweaks", // Must be unique. Same Id = one survives
    Name: "Tutorial Tweaks",
    Version: "1.1.0.0",
    Disables: { Id: "MadSkunky.TutorialJacobFix", Min: "0.0.0.0", Max: "1.0.0.0" },
    Description: "Fixes Jacob plus some additional tweaks for the tutorial",
    Lang: "en",
    Author: "MadSkunky",
    Url: "",
    Requires: { Id: "Modnix", Min: "3.0" },
    ConfigType: "MadSkunky.TutorialTweaks.ModConfig", // Fullname with namespace
}