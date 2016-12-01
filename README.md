# Ledybot
A pokemon SM giveaway bot for the n3ds. Needs NTR and Input Redirection to work.

Thanks to kwsch and the guys from PKHeX for their work on the .pk7 format!
Also thanks to Kazo for his NTR Input Redirection Client as well as Stary2001 for the actual .cia!

1. Boot your n3ds
2. Start the Boot NTR selector
3. Start the Input Redirection.cia
4. Start Pokemon SM and connect online in the Festival Plaza
5. Open the GTS until you see the "Seek Pokemon / Deposit Pokemon" screen
6. Start Ledybot, fill in your n3ds ip, ntr port and pokemon pid
7. Type in which pokemon the bot shall find in the GTS
8. Type in which pokemon you are giving away (this is used to check if we can actually trade the deposited pokemon)
9. Type in the lower level range of the pokemon you are trading. The bot will trade all "Any"-level pokemon as well as pokemon asking for this level+. This means if you want to trade a lvl 100 Ditto you have to put 91 in here (since the range goes 91 - 100).
10. Select a default .pk7 file, this file will be injected and traded by the bot on default.
11. Select a specific .pk7 folder. The bot will check the deposited Pokemons name, see if the name matches any .pk7 file in the folder and inject this one instead (if it exists).
12. Press Start

## Todo:

Lots of things!
- Comments and Documentation!
- Unicode support
- There may be some stability issues at times. I've had it run for 10h+ in the past but since the bot can't really restart itself when it crashes you still need to have an eye on it from time to time.
- 1-10 level range probably won't work yet because of lazy coding (You'd have to type in "1" in the field, but I use contains() to check... and 91 also contains 1 so yeaa...)
- Giving away different pokemon. This would need a simple change to check the requested pokemon based on the name of the deposited pokemon. Mostly a UI thing and I'm not really a fan of that, will come in the future.
- An actual working stop button (it will stop... eventually... maybe)
- Speed optimizations
- Prevent the same people from requesting more than once every X min. I already have the data of who requests (at least trainer name, country and subcountry), I'd just need to add them to a list and block them for a while. This would also block people from the same region with the same name though (unless I find a better way to identify who is requesting)
- Add an option for several deposited pokemon (to prevent people from redepositing the same pokemon all the time)?
- ... and more!
