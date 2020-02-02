using controllers;
using UnityEngine;
using Zenject;

public class StoryContainerInstaller : MonoInstaller
{
  public GameObject chickenPrefab;
  public GameObject firePrefab;
  public GameObject backgroundPrefab;
  public override void InstallBindings()
  {
    Container.Bind<StoryController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

    Container.Resolve<StoryController>().Init(chickenPrefab, firePrefab, backgroundPrefab); // not best practise but hey
  }
}

