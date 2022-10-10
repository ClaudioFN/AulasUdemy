describe("CookieDetector", function() {
    var itemsWithoutCookies;
    var itemsWithCookies;
  
    beforeEach(function() {
      itemsWithoutCookies = ['Apples', 'Pears', 'Bananas'];
      itemsWithCookies = ['Bread', 'Milk', 'Cookies'];
    });
  
    it("should be able to detect no cookies", function() {
      var result = detectCookie(itemsWithoutCookies);
      expect(result).not.toBe(true);
    });
  
    it("should be able to detect cookies", function() {
      var result = detectCookie(itemsWithCookies);
      expect(result).toBe(true);
    });
  });