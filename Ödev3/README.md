
# Veritabanı Konfigürasyonu ve CQRS Yapısı

Bu proje içerisinde aşağıdaki adımları izleyerek veritabanı konfigürasyonunu doğru bir şekilde ayarlamamız gerekmektedir:

1) "Address", "Note" ve "NoteCategory" varlıklarının içerisindeki alanları veritabanı ile uyumlu hâle getirmeliyiz.

2) Bir kullanıcının birden fazla adresi olabileceği için "User" ile "Address" arasında bir-e-çok (one-to-many) ilişki kurulmalıdır.

3) Bir notun birden fazla kategoriye ait olabileceği gibi, bir kategorinin de birden fazla nota atanabileceği için çok-a-çok (many-to-many) ilişki sağlanmalıdır.

4) "Address" için CQRS yapısına uygun olarak "Add", "Update", "Delete" ve "HardDelete" komutları oluşturulmalıdır.

5) "Address" için CQRS yapısına uygun olarak "GetById" ve "GetAll" sorguları oluşturulmalıdır.

Bu adımları takip ederek, proje içerisinde gerekli konfigürasyon ve yapılandırmaları tamamlayabiliriz.
