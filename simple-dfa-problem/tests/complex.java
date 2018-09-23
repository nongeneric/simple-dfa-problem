public class Main {
    public static void method(boolean... conditions) {
        int x;
        x = 0;
		x = 1;
		if (conditions[0]) {
			x = 2;
			x = 3;
			if (conditions[2]) {
				x = 5;
			}
			if (conditions[3]) {}
			if (conditions[4]) {}
			if (conditions[3]) {
				x = 6;
			}
		}
		if (conditions[5]) {
			x = 7;
		}
        System.out.println(x);
    }
}
