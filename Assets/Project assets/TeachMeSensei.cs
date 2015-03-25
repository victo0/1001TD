// CETTE CLASSE EST ICI POUR VOUS DONNER CERTAINES EXPLICATIONS / CONSEILS SUR LA MANIERE DE CODER.

//Il est conseillé d'importer ces 4 classes, elles sont utilisées par de nombreuses commandes de mon code.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD; //La collection TD est une collection que j'ai créé pour ce projet, elle comporte tout les scripts du dossier "TD".

//Premièrement, veuillez ne modifier les scripts de la liste uniquement si je vous ait demandé de le faire, histoire de ne pas avoir trop de problèmes de versionning.
//Le code as été réalisé de manière à ce vous puissez tout faire dans des scirpts enfants des classes que j'ai créé.

public class TeachMeSensei : MonoBehaviour {

	public void CreerUneUnité () {
		//	Creez un enfant de Unit.
		//	Copiez le code de Soldier.cs
		//  Modifiez ce que vous avez besoinde modifier.
		//  Bonus : Modifiez les valeurs de spawnpoint (dans Useweapon) pour changer l'endroit d'où spawn les projectiles. 
		//  Pour les effets à l'impact des attaques (slow/nerf des ennemis/ ...) Veuillez vous referez au "CreerUnProjectile" ci-dessous.

		}

	public void CreerUnProjectile () {
		// Creez un enfant de Projectile.
		// Copiez le code de SoldierProjectile.cs.
		// Modifiez/completez les différents ovveride en fonction des différents effets voulus.

		// Chaque effet de projectile (comme un slow par exemple) sera une fonction ajoutée à Unit.cs et apellée depuis le InflictDommage de votre projectile.
		// Une fois chaque nouvelle unité validé, j'ajouterais le ou les effets à ma version du Unit.cs.


		}

	public void VirtualEtOverride () {
		// De base, les fonctions d'une classe sont automatiquement lancées par les enfants de cette classe.
		// Placer une fonction en Virtual fait que cette fonction n'est pas jouée par les enfants. (A verifier, celà semble dépendre des cas pour une raison inconnue).

		// Utiliser override permet de remplacer une fonction public d'un parent.
		// Utiliser base.(nom de la fonction) permet de faire jouer la fonction de base, puis d'y ajouter toutes les lignes de l'override.

		// Regardez WorldObject.cs et Unit.cs pour des exemples en action.

		}

	public void CreerUnEnnemi () {
		// Creez un enfant de EnnemiUnit
		// Pour les effets n'impliquant pas les autres unités, placez les dans cette classe.

		// Pour éviter des (très) nombreux bugs que celà peut causer, les actions influant sur les autres unités (du joueur ou de l'ennemi) seront palcées dans Unit.cs
		// Tout comme pour les projectiles, ces effets seront appelés depuis l'ennemi, mais seront écris dans Unit.cs.
		// Tout comme pour les projectiles, je les ajouterais à ma version de Unit.cs une fois validés.

		// Exemple de bug que l'on évite ici : que se passe-t-il si on applique un effet sur le temps, et que le code se trouvait dans un ennemis qu'on as tué (et donc suprimé de la scène) ?

		}

}
