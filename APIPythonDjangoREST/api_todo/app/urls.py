from django.urls import path
# from app.views import TodoListAndCreate, TodoDetailChangeAndDelete # OLD MODE 2
from app.views import TodoViewSet
from rest_framework.routers import DefaultRouter # DEFINITIVE MODE

router = DefaultRouter()
router.register(r'', TodoViewSet)
urlpatterns = router.urls

# OLD MODE 2
# urlpatterns = [
#     path('', TodoListAndCreate.as_view()),
#     path('<int:pk>/', TodoDetailChangeAndDelete.as_view()),
# ]