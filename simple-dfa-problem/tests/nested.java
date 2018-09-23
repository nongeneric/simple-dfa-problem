public class Main {
    public static void method(boolean... conditions) {
        int x;
		x = 1;
        if (conditions[0])
			if (conditions[1])
				if (conditions[2])
					if (conditions[3])
						x = 2;
        System.out.println(x);
    }
}
