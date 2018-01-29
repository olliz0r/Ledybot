# Ledybot

## About
Ledybot is a GTS giveaway bot for pokemon sun and moon. Check out LedySync to go along with it, it's really cool: https://github.com/olliz0r/LedySync

If you are rich and like what you see, feel free to throw me a donation at https://www.paypal.me/olliz0r !

## Credits
Huge thanks to EzPz/imaboy321 for rewriting, enhancing and maintaining this project for so long!

## Usage
To get this to run you need a hacked 3DS with CFW. You need NTR (mode 3 version for o3ds) as well as Input Redirection (also mode 3 for o3ds)

1. Boot your 3DS
2. Start Boot NTR Selector
3. Start Input Redicretion
4. Start Pokemon, connect to the Festival Plaza to get online
5. Open the GTS until you see the "Seek Pokemon / Deposit Pokemon" screen
6. Start Ledybot, fill in your 3DS ip address
7. Fill in all necessary details (Deposited Pokemon, Giveaway Details)
8. Set settings to what you like (trade from front/back)
9. Connect!
10. Start the bot!

(For a slightly outdated, yet more up to date than the above guide check out: https://www.se7ensins.com/forums/threads/how-to-setup-a-pokemon-sun-and-moon-gts-giveaway-bot.1616037/ , thanks to ItsDeidara for letting me know!)

## Options
### GTS Tab
#### Deposited Pokemon:
- The three dropdown menus on the GTS tab are used for the bot to determine what pokemon should be found on the GTS.
#### Black List
- The black list checkbox prevents people from getting more than one pokemon, based on their FC
#### Giveaway Details
- This window is used to set up what kind of pokemon the bot should give out. Select a max count on how many pokemon should be traded (-1 for infinite), the dex number of the pokemon you are giving out, the levelrange of the pokemon (to prevent the bot from trying to trade a lvl 100 Ditto when a lvl 11-20 Ditto was requested, as this would result in a possible crash) and the gender of the pokemon you are giving out (put male for genderless pokemon). Select a .pk7 file in the Default box to trade. If you want to give out different versions of the same pokemon based on the name of the deposited pokemon, select a folder in the "Specific" box. The bot will then try to match the nickname of the pokemon to a file in that folder and trade this pokemon instead. For example, if someone deposits a Ledyba called "Adamant" asking for a lvl 91-100 Ditto, the bot would check the specific folder first for a "Adamant.pk7" file and trade this one, if such a file does not exist it would trade the default .pk7. The default box is optional in case you only want to trade pokemon that have a specific name.
#### Banned FCs
- You can add a list of FCs that should never be traded here
#### Reddit
- This feature combined with the Subreddit Textbox in the Settings tab can be used to only trade people that posted in a certain subreddits post and have their FC set up in their flair, check out EzPzs Guide https://www.youtube.com/watch?v=XrBEZ7qVaro for more information!
#### Trade List
- This ListView gives a summary of all people the bot has traded, including a timestamp, Trainer name and FC, Country, SubCountry, Pokemon recieved, Pokemon sent, GTS page and GTS listindex on this page they were on.
- You can Export/Import or Clear this list if you want to keep track of your trades outside of Ledybot itself.
### Injection Tab
- This tab is for quick injections of .pk7 or .wc7 files to the specified slot
### Breeding Tab
- This tab offers the option to force the pokemon daycare person to generate an egg
### Settings
#### Subreddit
- Used with the Reddit option of the GTS tab, check the Reddit feature above for more information
#### Trade direction
- You can set the bot to trade from the back of the last page, the back of the first page or from the front (first page only)
#### extra waittime (for o3ds)
- With the addition of stability checks the o3ds sometimes loads too slow and the bot tries to check too quickly for the current window state. Should the bot detect an incorrect window state it will try to recover. With this option you can delay the screen check a bit, giving the 3ds a bit more time to actually load the screen before it gets checked. A value of 1000ms works well for my o3ds, if your bot keeps going back to the initial screen (quit/seek/deposit screen) raise this value a bit
#### LedySync
- LedySync is a additional program used to sync the trades of several Ledybots together. If you have run several Ledybots on the same pokemon in the past you have noticed that once they start trying to trade the same person one Ledybot messes up and keeps not trading anyone. A usual solution was to let different instances of Ledybot either search from different directions, on different pokemon or for different pokemon. With LedySync you can sync up several instances of Ledybot to make sure that they don't trade the same person at the same time and that they have a shared blacklist, preventing people from getting one pokemon off of each Ledybot instance. Check it out at https://github.com/olliz0r/LedySync to find a short instruction on how to use it.

## Additional thanks
Thanks to kwsch and the guys from PKHeX for their work on the .pk7 format!
Also thanks to Kazo for his NTR Input Redirection Client as well as Stary2001 for the actual .cia!
