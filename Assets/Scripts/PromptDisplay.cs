using TMPro;
using UnityEngine;

// Shows a random prompt, then swaps to its category once the timer runs out.
public class PromptDisplay : MonoBehaviour
{
    public DrawTimer timer;
    public TMP_Text label;

    // "prompt, category"
    static readonly string[] prompts =
    {
        "british person, nationality",
        "french person, nationality",
        "australian person, nationality",
        "canadian person, nationality",
        "texan, nationality",
        "viking, historical figure",
        "caveman, historical figure",
        "napoleon, historical figure",
        "cleopatra, historical figure",
        "medieval knight, historical figure",
        "AI takeover of the world, apocalypses",
        "zombie apocalypse, apocalypses",
        "alien invasion, apocalypses",
        "meteor strike, apocalypses",
        "the floor is lava, apocalypses",
        "annoying orange, internet character",
        "rickroll, internet character",
        "doge, internet character",
        "nyan cat, internet character",
        "grumpy cat, internet character",
        "ponytail, hairstyle",
        "mullet, hairstyle",
        "mohawk, hairstyle",
        "bowl cut, hairstyle",
        "bed head, hairstyle",
        "monday morning, feeling",
        "existential dread, feeling",
        "hangry, feeling",
        "awkward silence, feeling",
        "stepping on a lego, feeling",
        "dad at a barbecue, stereotype",
        "gym bro, stereotype",
        "cat lady, stereotype",
        "tech startup founder, stereotype",
        "karen asking for the manager, stereotype",
        "haunted toaster, household object",
        "sentient couch, household object",
        "microwave with feelings, household object",
        "evil lamp, household object",
        "lonely sock, household object",
        "dog wearing a tie, animal",
        "pigeon with a job, animal",
        "cat plotting murder, animal",
        "goose on the loose, animal",
        "fish out of water, animal",
        "pizza with pineapple, food",
        "sad sandwich, food",
        "spaghetti monster, food",
        "burnt toast, food",
        "spicy noodles, food",
        "superhero on vacation, superhero",
        "villain doing taxes, superhero",
        "retired superhero, superhero",
        "sidekick getting fired, superhero",
        "superhero laundry day, superhero",
        "wizard, fantasy creature",
        "dragon hoarding snacks, fantasy creature",
        "mermaid on land, fantasy creature",
        "goblin, fantasy creature",
        "unicorn with a hangover, fantasy creature",
        "group project, school life",
        "pop quiz, school life",
        "cafeteria food, school life",
        "detention, school life",
        "the teacher's pet, school life",
        "traffic jam, everyday disaster",
        "dropped ice cream, everyday disaster",
        "phone at 1 percent, everyday disaster",
        "wifi down, everyday disaster",
        "locked out of the house, everyday disaster",
        "the moon, space",
        "black hole, space",
        "astronaut lost in space, space",
        "UFO abduction, space",
        "martian tourist, space",
        "time traveler, sci-fi",
        "robot butler, sci-fi",
        "clone gone wrong, sci-fi",
        "cyborg grandma, sci-fi",
        "teleporter accident, sci-fi",
        "first date, life event",
        "wedding crasher, life event",
        "job interview, life event",
        "moving day, life event",
        "birthday surprise, life event",
        "ninja, occupation",
        "pirate, occupation",
        "mime, occupation",
        "clown, occupation",
        "lifeguard, occupation",
        "volcano, nature",
        "tornado, nature",
        "thunderstorm, nature",
        "quicksand, nature",
        "deserted island, nature",
        "ghost, spooky thing",
        "vampire at the dentist, spooky thing",
        "haunted house, spooky thing",
        "skeleton workout, spooky thing",
        "witch's cauldron, spooky thing",
    };

    string prompt;
    string category;

    void Start()
    {
        string line = prompts[Random.Range(0, prompts.Length)];
        int split = line.LastIndexOf(',');
        prompt = line.Substring(0, split).Trim();
        category = line.Substring(split + 1).Trim();
        label.text = prompt;
    }

    void Update()
    {
        if (timer.IsDone) label.text = category;
    }
}
