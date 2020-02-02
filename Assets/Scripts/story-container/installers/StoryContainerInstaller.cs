using controllers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class StoryContainerInstaller : MonoInstaller
{
  public GameObject motherCluckerPrefab;
  public GameObject lankyPrefab;
  public GameObject cluckingtonPrefab;
  public GameObject flambeauPrefab;
  public GameObject firePrefab;
  public GameObject backgroundPrefab;
  public override void InstallBindings()
  {
    Container.Bind<StoryController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

    Container.Resolve<StoryController>().Init(
      motherCluckerPrefab,
      lankyPrefab, 
      cluckingtonPrefab, 
      flambeauPrefab, 
      firePrefab, 
      backgroundPrefab); // not best practise but hey
  }
}

