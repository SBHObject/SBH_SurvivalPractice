# 던전탐험 만들기
 내일배움캠프 유니티숙련주차 3D 프로젝트

![기본 이미지](https://laced-rutabaga-712.notion.site/image/https%3A%2F%2Fprod-files-secure.s3.us-west-2.amazonaws.com%2Ff3d7f86c-cdab-4d84-9092-b767f79f7186%2Fb001cb97-8ae4-4c57-a71c-bcb437da5550%2F%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7(1133).png?table=block&id=12f43507-6cd3-80d3-afbb-ffea6487e30e&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&width=1360&userId=&cache=v2)

- 기능위주로 구현된 프로젝트입니다.

### 조작
![이동 조작](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/b7cb38ae-371a-4182-adac-40b708c892c9/MoveAndJump.gif?table=block&id=12f43507-6cd3-800b-b306-ff3ac5b006b3&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=7GBZgeOF7Qz0OTNOWOgMMwytxD_0xTIJ0jinhuFk4Ow)

이동 : WASD 를 사용하여 이동합니다
점프 : 스페이스바를 사용하여 점프합니다.

![벽타기](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/936c8580-8e63-4ca7-9fd9-8d7de2dd504b/Clamb.gif?table=block&id=12f43507-6cd3-8076-a009-f0f3d847b835&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=90cyZs2-O4BQc6KVdiMepzNyV8ilNKS6ng9c-x1jNJc)

벽타기 : 벽앞, 공중에서 점프를 두번 눌러 벽을 탈 수 있습니다.

- 벽타기중, W,S 조작으로 위 아래이동, AD 조작으로 좌우 이동을 할 수 있습니다.

![상호작용](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/20d98227-ab18-4b52-974b-e25acd68efa8/Interaction.gif?table=block&id=12f43507-6cd3-8029-bac5-c10a5a424ee9&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=41KrW19q3b23BKmcZutK44poi7SephZNksEshN4Jbeg)

상호작용 : E 키를 눌러 상호작용 가능한 오브젝트에 상호작용 할 수 있습니다.

![아이템 장착](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/c4e26f0a-3bc5-4836-9c38-05b5d418f170/Item.gif?table=block&id=12f43507-6cd3-805f-858e-ea027f1110d9&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=SXit3ln27c-JPmJt_xbM0YtDvCfe8ziBMoKWFDArSpA)

아이템 장착 : Tab 키를 눌러 인벤토리를 열 수 있습니다. 
- 인벤토리에서 아이템 선택 후, 버튼을 눌러 장착, 해제, 버리기, 사용 을 할 수 있습니다.

### 기능

![플랫폼](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/a51076ab-7200-41a7-88f4-8fdb5a7f30d6/Platform.gif?table=block&id=12f43507-6cd3-80ce-9139-c76c0be2f14e&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=kBI66fPsK0XvoWfLTgxqigj-sjuyNtGJI6pjVW4Beao)

플랫폼을 타고 이동할 수 있습니다.

![캐논](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/857d1b1b-fa59-4076-9b99-e17330434026/Canon.gif?table=block&id=12f43507-6cd3-80df-a7ce-edd2c1c505cb&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=cYkScizM5DmETCNhgRMU7YaR8mbd67aEmJFkTE6Cymc)

캐논을 사용하여 지정된 방향으로 날라갈 수 있습니다.

![장비](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/4cfac3c3-9825-4751-acef-7b2944ae4ba3/ItemStat.gif?table=block&id=12f43507-6cd3-8024-8c07-dde01bb4fe9a&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=JiEsvOoGO5usEbM2tQSzVPrL_cHnl6-ZPSkORLs_-74)

아이템을 장착하여, 스텟에 변화를 줄 수 있습니다.

![포탑](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/897f8169-a60b-4b7a-bca6-7d80027f17bb/Turret.gif?table=block&id=12f43507-6cd3-80b5-aa4e-e04c2dbdad9d&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1730340000000&signature=RkXV98MPkFm_CpNE9Gbipns1eWVRFqkEIbnwfApO990)

포탑이 발사하는 레이저에 닿으면 포탑이 공격합니다
