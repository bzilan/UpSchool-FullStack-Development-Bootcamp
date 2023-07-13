
# Unit Testler

Bu proje içerisinde UserServiceTests sınıfında aşağıdaki adımları içeren test metotları kullanılarak kullanıcı hizmetlerinin doğru bir şekilde çalışıp çalışmadığı test edilmektedir:

1)AddAsync_ShouldThrowException_WhenEmailIsEmptyOrNull(): E-posta boş veya null olduğunda bir istisna fırlatılıp fırlatılmadığını kontrol eder.

2)AddAsync_ShouldReturn_CorrectUserId(): Doğru bir kullanıcı kimliği döndürüp döndürmediğini kontrol eder.

3)DeleteAsync_ShouldReturnTrue_WhenUserExists(): Kullanıcı mevcut olduğunda true değeri döndürüp döndürmediğini kontrol eder.

4)DeleteAsync_ShouldThrowException_WhenUserDoesntExists(): Kullanıcı mevcut olmadığında bir istisna fırlatılıp fırlatılmadığını kontrol eder.

5)UpdateAsync_ShouldThrowException_WhenUserIdIsEmpty(): Kullanıcı kimliği boş olduğunda bir istisna fırlatılıp fırlatılmadığını kontrol eder.

6)UpdateAsync_ShouldThrowException_WhenUserEmailEmptyOrNull(): Kullanıcı e-postası boş veya null olduğunda bir istisna fırlatılıp fırlatılmadığını kontrol eder.

7)GetAllAsync_ShouldReturn_UserListWithAtLeastTwoRecords(): En az iki kayıt içeren bir kullanıcı listesi döndürüp döndürmediğini kontrol eder.

Bu testlerde FakeItEasy kütüphanesinden faydalanılmaktadır.