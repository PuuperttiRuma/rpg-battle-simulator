using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CharacterCreator : MonoBehaviour {

    public InputField combatantNameField;
    public Slider stressSlider;
    public Slider consequencesSlider;
    public Slider fightAttributeSlider;
    public Slider fightSkillSlider;
    public InputField attackBonusField;
    public InputField defenseBonusField;
    public InputField armorValueField;
    public InputField weaponDamageField;
    public Button createButton;
    public Text warningText;

    Text stressTitle;
    Text conseqsTitle;
    Text attributeTitle;
    Text skillTitle;
    List<InputField> fields;

    void Awake()
    {
        stressTitle = stressSlider.GetComponentInChildren<Text>();
        conseqsTitle = consequencesSlider.GetComponentInChildren<Text>();
        attributeTitle = fightAttributeSlider.GetComponentInChildren<Text>();
        skillTitle = fightSkillSlider.GetComponentInChildren<Text>();
        fields = new List<InputField>();
        fields.Add(attackBonusField);
        fields.Add(defenseBonusField);
        fields.Add(armorValueField);
        fields.Add(weaponDamageField);
        updateTexts();
        checkFields();
    }

    public void updateTexts()
    {
        stressTitle.text = "Stress (" + (int)Math.Round(stressSlider.value) + "):";
        conseqsTitle.text = "Conseqs (" + (int)Math.Round(consequencesSlider.value)*2 + "):";
        attributeTitle.text = "Attribute (" + (int)Math.Round(fightAttributeSlider.value) + "):";
        skillTitle.text = "Skill (" + (int)Math.Round(fightSkillSlider.value) + "):";
    }

    public void checkFields()
    {
        int value;
        foreach (InputField i in fields)
        {
            if (Int32.TryParse(i.text, out value))
            {
                warningText.text = "";
                createButton.interactable = true;
            }
            else
            {
                warningText.text = "Modifiers can only be whole numbers.";
                createButton.interactable = false;
                return;
            }
        }

    }


}
